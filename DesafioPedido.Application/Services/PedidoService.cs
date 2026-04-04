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
            var itenPedido = await _pedidoRepository.GetByIdAsync(pedidoDto.PedidoId);

            // REMOVER OS ITENS
            await _pedidoRepository.GetItenByIdAsync(pedidoDto.PedidoId);

            var pedido = new Pedido
            {
                PedidoId = pedidoDto.PedidoId,
                ClienteId = pedidoDto.ClienteId,
                DataPedido = DateTime.Now,
                Status = "Atualizado",
            };

            // adicionar os novos itens
            foreach (var item in pedidoDto.Itens)
            {
                var itemPedido = new ItemPedido
                {
                    PedidoId = itenPedido.PedidoId,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario
                };

                // até aqui esta funcionando!
                // add os novos itens e salva na tabela
                pedido.Itens.Add(itemPedido);
                await _pedidoRepository.AddItemAsync(itemPedido);

                // atualiza o estoque do produto
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);
                produto.QuantidadeEstoque -= item.Quantidade;
                await _produtoRepository.UpdateAsync(produto);
            }

            pedido.AtualizarTotal();
            await _pedidoRepository.UpdateAsync(pedido);

            // atualiza o valor total do pedido
            await _pedidoRepository.UpdateTotalAsync(pedido.PedidoId, pedido.ValorTotal);

        }
    }
}
