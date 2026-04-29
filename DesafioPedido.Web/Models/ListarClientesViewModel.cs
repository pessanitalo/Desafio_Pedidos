using DesafioPedido.Application.DTOs;

namespace DesafioPedido.Web.Models
{
    public class ListarClientesViewModel
    {
        public IEnumerable<ClienteDTO> Clientes { get; set; } = new List<ClienteDTO>();
        public string? NomeFiltro { get; set; }
        public string? EmailFiltro { get; set; }
    }
}
