namespace DesafioPedido.Domain.Entities
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descrição { get; set; }
        public string Preço { get; set; }
        public int QuantidadeEstoque { get; set; }

        public Produto() { }

    }
}
