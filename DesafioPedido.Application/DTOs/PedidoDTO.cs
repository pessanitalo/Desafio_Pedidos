namespace DesafioPedido.Application.DTOs
{
    public class PedidoDTO
    {
        public int ClienteId { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; } = new();
    }
}
