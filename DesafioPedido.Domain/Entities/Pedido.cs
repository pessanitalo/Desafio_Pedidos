namespace DesafioPedido.Domain.Entities
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string  Status { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public List<ItemPedido> Itens { get; set; } = new();

        public decimal CalcularTotal()
        {
            return Itens.Sum(item => item.PrecoUnitario * item.Quantidade);
        }

        public void AtualizarTotal()
        {
            ValorTotal = CalcularTotal();
        }
    }
}
