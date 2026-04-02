using DesafioPedido.Application.DTOs;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Validation;

namespace DesafioPedido.Application.Interfaces
{
    public interface IProdutoInterface
    {
        Task<Result<string>> AddAsync(ProdutoDto clienteDTO);
        Task<Result<IEnumerable<Produto>>> GetAllAsync();
        Task<Result<IEnumerable<Produto>>> GetProdutosDisponiveisAsync();
        Task<Result<Produto>> GetByIdAsync(int id);
        Task<Result<string>> UpdateAsync(ProdutoDto clienteDTO);
        Task<Result<string>> DeleteAsync(int id);
    }
}
