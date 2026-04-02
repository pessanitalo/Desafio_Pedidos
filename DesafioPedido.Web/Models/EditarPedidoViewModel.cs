using DesafioPedido.Application.DTOs;
using DesafioPedido.Domain.Entities;
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
        public IEnumerable<Cliente> Clientes { get; set; }
        [ValidateNever]
        public IEnumerable<Produto> Produtos { get; set; }
    }
}
