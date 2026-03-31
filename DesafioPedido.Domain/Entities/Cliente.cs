namespace DesafioPedido.Domain.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }

        public IEnumerable<Pedido> Pedido { get; set; }

        public Cliente() { }

        public Cliente(string nome, string email, string telefone, DateTime dataCadastro)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone;
            DataCadastro = dataCadastro;
        }
    }
}
