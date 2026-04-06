namespace DesafioPedido.Application.DTOs
{
    public class ItemPedidoDTO
    {
        public int ItemId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
