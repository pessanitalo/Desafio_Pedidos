using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.DTOs;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Interfaces;
using DesafioPedido.Domain.Validation;
using Microsoft.Extensions.Logging;


namespace DesafioPedido.Application.Services
{
    public class PedidoService : IPedidoInterface
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly ILogger<PedidoService> _logger;

        public PedidoService(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository, ILogger<PedidoService> logger)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        public async Task<Result<string>> AddAsync(PedidoDTO pedidoDTO)
        {
            try
            {
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

                _logger.LogInformation(
                    "[NOTIFICAÇÃO] Pedido #{PedidoId} criado com sucesso. Status definido como '{Status}' em {DataPedido}.",
                    pedidoId, pedido.Status, pedido.DataPedido);

                foreach (var item in pedidoDTO.Itens)
                {
                    var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);

                    var itemPedido = criarItemPedido(pedidoId, item, null);

                    pedido.Itens.Add(itemPedido);
                    await _pedidoRepository.AddItemAsync(itemPedido);
                    await atualizarEstoqueProduto(produto, item.Quantidade);
                }

                pedido.AtualizarTotal();
                await _pedidoRepository.UpdateTotalAsync(pedidoId, pedido.ValorTotal);

                return Result<string>.Ok("Pedido criado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                  "[NOTIFICAÇÃO] Falha ao criar pedido para o cliente {ClienteId}.",
                  pedidoDTO.ClienteId);
                return Result<string>.Fail($"Não foi possível criar o pedido: {ex.Message}");
            }
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            try
            {
                var cliente = await _pedidoRepository.GetByIdAsync(id);

                if (cliente is null)
                    return Result<string>.Fail("Pedido não encontrado.");

                await _pedidoRepository.DeleteAsync(id);
                return Result<string>.Ok("Pedido excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Não foi possível excluir o pedido: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PedidoResumoDTO>>> GetAllAsync(int? clienteId, string? status)
        {
            try
            {
                var pedidos = await _pedidoRepository.GetAllAsync(clienteId, status);
                return Result<IEnumerable<PedidoResumoDTO>>.Ok(pedidos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PedidoResumoDTO>>.Fail($"Não foi possível carregar os pedidos: {ex.Message}");
            }
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

        public async Task<Result<string>> UpdateAsync(EditarPedidoDTO pedidoDto)
        {
            try
            {
                if (pedidoDto.ClienteId == 0)
                    return Result<string>.Fail("Você deve selecionar um cliente.");

                foreach (var item in pedidoDto.Itens)
                {
                    if (item.ProdutoId == 0) return Result<string>.Fail("Você deve selecionar um produto.");
                }
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
                    pedido.Status = "Processando";
                    pedido.DataPedido = DateTime.Now;

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

                            pedido.Itens.Add(itemPedido);
                            await _pedidoRepository.AddItemAsync(itemPedido);

                            // atualiza o estoque
                            var produto = await _produtoRepository.GetByIdAsync(itemPedido.ProdutoId);
                            await atualizarEstoqueProduto(produto, itemPedido.Quantidade);
                        }
                        else if (itemExistente.Quantidade != itemDto.Quantidade)
                        {
                            // Atualiza o item
                            await _pedidoRepository.UpdateItemAsync(itemDto.ItemId, itemDto.Quantidade);

                            var item = criarItemPedido(pedido.PedidoId, itemDto, itemDto.ItemId);

                            pedido.Itens.Add(item);

                            // atualiza o estoque
                            var produto = await _produtoRepository.GetByIdAsync(itemExistente.ProdutoId);
                            var estoqueAtual = item.Quantidade - itemExistente.Quantidade;
                            await atualizarEstoqueProduto(produto, estoqueAtual);
                        }

                    }

                    // Recarrega TODOS os itens do banco
                    pedido.Itens = await _pedidoRepository.GetItensByIdAsync(pedido.PedidoId);

                    pedido.AtualizarTotal();
                    await _pedidoRepository.UpdateAsync(pedido);

                    // atualiza o valor total do pedido
                    await _pedidoRepository.UpdateTotalAsync(pedido.PedidoId, pedido.ValorTotal);

                }
                return Result<string>.Ok("Pedido editado com sucesso.");

            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Não foi possível editar o pedido: {ex.Message}");
            }

        }

        private async Task atualizarEstoqueProduto(Produto produto, int quantidade)
        {
            produto.QuantidadeEstoque -= quantidade;
            await _produtoRepository.UpdateAsync(produto);
        }

        private ItemPedido criarItemPedido(int pedidoId, ItemPedidoDTO itemDto, int? itemId = null)
        {
            return new ItemPedido
            {
                ItemId = itemId ?? 0,
                PedidoId = pedidoId,
                ProdutoId = itemDto.ProdutoId,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = itemDto.PrecoUnitario
            };
        }
    }
}
