using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task CreateAsync(Produto cliente);
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<IEnumerable<Produto>> GetProdutosDisponiveisAsync();
        Task<Produto> GetByIdAsync(int id);
        Task UpdateAsync(Produto cliente);
        Task UpdadeBalanceProductAsync(int produtoId, int quantidade);
        Task DeleteAsync(int id);
    }
}
