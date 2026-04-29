using DesafioPedido.Domain.DTOs;
using DesafioPedido.Domain.Entities;

namespace DesafioPedido.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<int> CreateAsync(Pedido pedido);
        // mover para ItemPedidoRepository
        Task DeleteItenByIdAsync(int id);
        Task <List<ItemPedido>> GetItensByIdAsync(int id);
        Task AddItemAsync(ItemPedido item);
        Task UpdateItemAsync(int id, int quantidade);
        //
        Task<Pedido> GetPedidoByIdAsync(int id);
        Task UpdateAsync(Pedido pedido);
        Task UpdateTotalAsync(int pedidoId, decimal valorTotal);
        Task<IEnumerable<PedidoResumoDTO>> GetAllAsync(int? clienteId, string? status);
        Task DeleteAsync(int id);
        Task<PedidoDetalhesDTO> GetByIdAsync(int id);
    }
}
