namespace DesafioPedido.Domain.DTOs
{
    public class ItemPedidoDetalhesDTO
    {
        public int ItemId { get; set; }
        public string NomeProduto { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
