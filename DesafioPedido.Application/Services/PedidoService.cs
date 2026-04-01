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
                return Result<string>.Fail("Selecione um cliente.");

            foreach (var item in pedidoDTO.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);

                if (produto is null)
                    return Result<string>.Fail($"Produto {item.ProdutoId} não encontrado.");

                if (produto.QuantidadeEstoque < item.Quantidade)
                    return Result<string>.Fail($"Estoque insuficiente para o produto {produto.Nome}. Disponível: {produto.QuantidadeEstoque}");
            }

            // Calcula total e salva
            decimal valorTotal = 0;
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
                    PedidoId = pedidoId, // aqui também passando o ai
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = produto.Preco
                };

                valorTotal += produto.Preco * item.Quantidade;

                await _pedidoRepository.AddItemAsync(itemPedido);

                // Desconta do estoque
                produto.QuantidadeEstoque -= item.Quantidade;
                await _produtoRepository.UpdateAsync(produto);
            }

            await _pedidoRepository.UpdateTotalAsync(pedidoId, valorTotal); 

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
    }
}
