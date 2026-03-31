using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task CreateAsync(Produto cliente);
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<Produto> GetByIdAsync(int id);
        Task UpdateAsync(Produto cliente);
        Task DeleteAsync(int id);
    }
}
