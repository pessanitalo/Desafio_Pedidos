namespace DesafioPedido.Domain.DTOs
{
    public class PedidoDetalhesDTO
    {
        public int PedidoId { get; set; }
        public string NomeCliente { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; }
        public List<ItemPedidoDetalhesDTO> Itens { get; set; } = new();
    }
}
