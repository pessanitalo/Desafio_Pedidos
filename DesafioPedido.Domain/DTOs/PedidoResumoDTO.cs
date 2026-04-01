namespace DesafioPedido.Domain.DTOs
{
    public class PedidoResumoDTO
    {
        public int PedidoId { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; }
    }
}
