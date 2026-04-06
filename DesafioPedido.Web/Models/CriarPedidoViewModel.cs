using DesafioPedido.Application.DTOs;
using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Web.Models
{
    public class CriarPedidoViewModel
    {
        public PedidoDTO Pedido { get; set; } = new();
        public int ClienteId { get; set; }
        public IEnumerable<ClienteDTO> Clientes { get; set; } = new List<ClienteDTO>();
        public IEnumerable<ProdutoDto> Produtos { get; set; } = new List<ProdutoDto>();
    }
}
