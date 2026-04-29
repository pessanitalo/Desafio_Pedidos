using DesafioPedido.Application.DTOs;
using DesafioPedido.Domain.Validation;

namespace DesafioPedido.Application.Interfaces
{
    public interface IProdutoInterface
    {
        Task<Result<string>> AddAsync(ProdutoDto clienteDTO);
        Task<Result<IEnumerable<ProdutoDto>>> GetAllAsync();
        Task<Result<IEnumerable<ProdutoDto>>> GetProdutosDisponiveisAsync();
        Task<Result<ProdutoDto>> GetByIdAsync(int id);
        Task<Result<string>> UpdateAsync(ProdutoDto clienteDTO);
        Task<Result<string>> DeleteAsync(int id);
    }
}
