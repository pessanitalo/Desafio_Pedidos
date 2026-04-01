using DesafioPedido.Domain.DTOs;
using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<int> CreateAsync(Pedido pedido);
        Task AddItemAsync(ItemPedido item);
        Task UpdateTotalAsync(int pedidoId, decimal valorTotal);
        Task<IEnumerable<PedidoResumoDTO>> GetAllAsync(int? clienteId, string? status);
        Task UpdateAsync(Pedido cliente);
        Task DeleteAsync(int id);
        Task<PedidoDetalhesDTO> GetByIdAsync(int id);
    }
}
