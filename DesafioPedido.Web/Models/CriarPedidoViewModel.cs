using DesafioPedido.Application.DTOs;
using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Web.Models
{
    public class CriarPedidoViewModel
    {
        public PedidoDTO Pedido { get; set; } = new();
        public IEnumerable<Cliente> Clientes { get; set; } = new List<Cliente>();
        public IEnumerable<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
