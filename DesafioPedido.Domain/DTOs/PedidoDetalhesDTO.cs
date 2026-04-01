using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioPedido.Domain.DTOs
{
    public class PedidoDetalhesDTO
    {
        public int PedidoId { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; }
        public List<ItemPedidoDetalhesDTO> Itens { get; set; } = new();
    }
}
