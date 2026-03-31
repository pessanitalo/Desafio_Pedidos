namespace DesafioPedido.Application.DTOs
{
    public class ProdutoDto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
