using DesafioPedido.Application.DTOs;
using DesafioPedido.Domain.DTOs;

using DesafioPedido.Domain.Validation;

namespace DesafioPedido.Application.Interfaces
{
    public interface IPedidoInterface
    {
        Task<Result<IEnumerable<PedidoResumoDTO>>> GetAllAsync(int? clienteId, string? status);
        Task<Result<string>> AddAsync(PedidoDTO pedidoDTO);
        Task<Result<PedidoDetalhesDTO>> GetByIdAsync(int id);
        Task<Result<string>> DeleteAsync(int id);
    }
}
