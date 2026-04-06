using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Application.DTOs
{
    public class CriarPedidoDTO
    {
        public PedidoDTO Pedido { get; set; } = new();
        public int ClienteId { get; set; }
        public IEnumerable<ClienteDTO> Clientes { get; set; } = new List<ClienteDTO>();
        public IEnumerable<ProdutoDto> Produtos { get; set; } = new List<ProdutoDto>();
    }
}
