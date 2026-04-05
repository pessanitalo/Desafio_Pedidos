using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task CreateAsync(Produto produto);
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<IEnumerable<Produto>> GetProdutosDisponiveisAsync();
        Task<Produto> GetByIdAsync(int id);
        Task UpdateAsync(Produto produto);
        Task UpdadeBalanceProductAsync(int produtoId, int quantidade);
        Task DeleteAsync(int id);
    }
}
