using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.DTOs;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Interfaces;
using DesafioPedido.Domain.Validation;


namespace DesafioPedido.Application.Services
{
    public class PedidoService : IPedidoInterface
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;

        public PedidoService(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<Result<string>> AddAsync(PedidoDTO pedidoDTO)
        {
            // Valida estoque de cada item
            if (pedidoDTO.ClienteId == 0)
                return Result<string>.Fail("Você deve selecionar um cliente.");

            foreach (var item in pedidoDTO.Itens)
            {
                if (item.ProdutoId == 0) return Result<string>.Fail("Você deve selecionar um produto.");
            }


            foreach (var item in pedidoDTO.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);

                if (produto is null)
                    return Result<string>.Fail($"Produto {item.ProdutoId} não encontrado.");

                if (produto.QuantidadeEstoque < item.Quantidade)
                    return Result<string>.Fail($"Estoque insuficiente para o produto {produto.Nome}. Disponível: {produto.QuantidadeEstoque}");
            }

            // Calcula total e salva
            var pedido = new Pedido
            {
                ClienteId = pedidoDTO.ClienteId,
                DataPedido = DateTime.Now,
                Status = "Novo"
            };

            var pedidoId = await _pedidoRepository.CreateAsync(pedido);

            foreach (var item in pedidoDTO.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);

                var itemPedido = new ItemPedido
                {
                    PedidoId = pedidoId,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = produto.Preco
                };

                pedido.Itens.Add(itemPedido);
                await _pedidoRepository.AddItemAsync(itemPedido);

                produto.QuantidadeEstoque -= item.Quantidade;
                await _produtoRepository.UpdateAsync(produto);
            }

            pedido.AtualizarTotal();
            await _pedidoRepository.UpdateTotalAsync(pedidoId, pedido.ValorTotal);

            return Result<string>.Ok("Pedido criado com sucesso.");
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            var cliente = await _pedidoRepository.GetByIdAsync(id);

            if (cliente is null)
                return Result<string>.Fail("Pedido não encontrado.");

            await _pedidoRepository.DeleteAsync(id);
            return Result<string>.Ok("Pedido excluído com sucesso.");
        }

        public async Task<Result<IEnumerable<PedidoResumoDTO>>> GetAllAsync(int? clienteId, string? status)
        {
            var pedidos = await _pedidoRepository.GetAllAsync(clienteId, status);
            return Result<IEnumerable<PedidoResumoDTO>>.Ok(pedidos);
        }

        public async Task<Result<PedidoDetalhesDTO>> GetByIdAsync(int id)
        {
            try
            {
                var pedido = await _pedidoRepository.GetByIdAsync(id);

                if (pedido is null)
                    return Result<PedidoDetalhesDTO>.Fail("Pedido não encontrado.");

                return Result<PedidoDetalhesDTO>.Ok(pedido);
            }
            catch (Exception ex)
            {
                return Result<PedidoDetalhesDTO>.Fail($"Erro ao buscar pedido: {ex.Message}");
            }
        }

        public async Task UpdateAsync(EditarPedidoDTO pedidoDto)
        {

            var historicoItens = await _pedidoRepository.GetItensByIdAsync(pedidoDto.PedidoId);

            bool itensIguais = historicoItens.Select(x => new { x.ProdutoId, x.Quantidade })
                .SequenceEqual(pedidoDto.Itens.Select(x => new { x.ProdutoId, x.Quantidade }));

            var itensRemovidos = historicoItens
                   .Where(antigo => !pedidoDto.Itens.Any(novo => novo.ProdutoId == antigo.ProdutoId))
                   .ToList();


  
            foreach (var item in itensRemovidos)
            {
                await _pedidoRepository.DeleteItenByIdAsync(item.ProdutoId);
            }

            if (!itensIguais)
            {
                var pedido = await _pedidoRepository.GetPedidoByIdAsync(pedidoDto.PedidoId);

                pedido.ClienteId = pedidoDto.ClienteId;
                pedido.Status = "Atualizado";
                pedido.DataPedido = DateTime.Now;


                // adicionar os novos itens
                foreach (var itemDto in pedidoDto.Itens)
                {
                    var itemExistente = historicoItens.FirstOrDefault(x => x.ProdutoId == itemDto.ProdutoId);

                    if (itemExistente == null)
                    {
                        var itemPedido = new ItemPedido
                        {
                            PedidoId = pedido.PedidoId,
                            ProdutoId = itemDto.ProdutoId,
                            Quantidade = itemDto.Quantidade,
                            PrecoUnitario = itemDto.PrecoUnitario
                        };

                        // até aqui esta funcionando!
                        pedido.Itens.Add(itemPedido);
                        await _pedidoRepository.AddItemAsync(itemPedido);
                    }
                    else if (itemExistente.Quantidade != itemDto.Quantidade)
                    {
                        // precisa atualizar o item para calcular o total
                        await _pedidoRepository.UpdateItemAsync(itemDto.ItemId, itemDto.Quantidade);

                        var item = new ItemPedido
                        {
                            ItemId = itemDto.ItemId,
                            ProdutoId = itemDto.ProdutoId,
                            Quantidade = itemDto.Quantidade,
                            PrecoUnitario = itemDto.PrecoUnitario
                        };

                        pedido.Itens.Add(item);

                        // atualiza o estoque
                        var produto = await _produtoRepository.GetByIdAsync(itemExistente.ProdutoId);
                        var estoqueAtual = item.Quantidade - itemExistente.Quantidade;
                        produto.QuantidadeEstoque -= estoqueAtual;
                        await _produtoRepository.UpdateAsync(produto);
                    }

                }

                // Recarrega TODOS os itens do banco
                pedido.Itens = await _pedidoRepository.GetItensByIdAsync(pedido.PedidoId);

                pedido.AtualizarTotal();
                await _pedidoRepository.UpdateAsync(pedido);

                // atualiza o valor total do pedido
                await _pedidoRepository.UpdateTotalAsync(pedido.PedidoId, pedido.ValorTotal);
                Console.WriteLine($"Itens: {pedido.Itens.Count}");
            }

        }
    }
}
