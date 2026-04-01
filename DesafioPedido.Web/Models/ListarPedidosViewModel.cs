using DesafioPedido.Domain.DTOs;
using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Web.Models
{
    public class ListarPedidosViewModel
    {
        public IEnumerable<PedidoResumoDTO> Pedidos { get; set; } = new List<PedidoResumoDTO>();
        public IEnumerable<Cliente> Clientes { get; set; } = new List<Cliente>();
        public int? ClienteIdFiltro { get; set; }
        public string? StatusFiltro { get; set; }
    }
}
