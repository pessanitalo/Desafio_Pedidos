namespace DesafioPedido.Domain.Entities
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }

        public Produto(string nome, string descricao, decimal preco, int quantidadeEstoque)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
        }

        public Produto() { }


    }
}
