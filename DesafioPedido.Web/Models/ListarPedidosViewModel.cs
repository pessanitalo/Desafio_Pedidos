using DesafioPedido.Application.DTOs;
using DesafioPedido.Domain.DTOs;
using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Web.Models
{
    public class ListarPedidosViewModel
    {
        public IEnumerable<PedidoResumoDTO> Pedidos { get; set; } = new List<PedidoResumoDTO>();
        public IEnumerable<ClienteDTO> Clientes { get; set; } = new List<ClienteDTO>();
        public int? ClienteIdFiltro { get; set; }
        public string? StatusFiltro { get; set; }
    }
}
