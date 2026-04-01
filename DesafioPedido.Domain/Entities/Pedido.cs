namespace DesafioPedido.Domain.Entities
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string  Status { get; set; } // alterar o tipo

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
