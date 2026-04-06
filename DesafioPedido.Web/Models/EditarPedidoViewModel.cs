using DesafioPedido.Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DesafioPedido.Web.Models
{
    public class EditarPedidoViewModel
    {
        public int PedidoId { get; set; }

        public int ClienteId { get; set; }

        public List<ItemPedidoViewModel> Itens { get; set; } = new();

        // Listas para dropdown
        [ValidateNever]
        public IEnumerable<ClienteDTO> Clientes { get; set; } // AQUI PODE SER DTO ?
        [ValidateNever]
        public IEnumerable<ProdutoDto> Produtos { get; set; }
    }
}
