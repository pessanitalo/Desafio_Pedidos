namespace DesafioPedido.Web.Models
{
    public class ProdutoViewModel
    {
        public int ProdutoId { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public decimal Preco { get; set; }

        public int QuantidadeEstoque { get; set; }
    }
}
