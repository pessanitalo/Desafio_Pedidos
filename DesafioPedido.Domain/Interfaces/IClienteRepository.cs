using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task CreateAsync(Cliente cliente);
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente> GetByIdAsync(int id);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(int id);
    }
}
