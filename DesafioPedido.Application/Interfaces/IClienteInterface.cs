using DesafioPedido.Application.DTOs;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Validation;

namespace DesafioPedido.Application.Interfaces
{
    public interface IClienteInterface
    {
        Task<Result<string>> AddAsync(ClienteDTO clienteDTO);
        Task<Result<IEnumerable<Cliente>>> GetAllAsync();
        Task<Result<Cliente>> GetByIdAsync(int id);
        Task<Result<string>> UpdateAsync(ClienteDTO clienteDTO);
        Task<Result<string>> DeleteAsync(int id);
    }
}
