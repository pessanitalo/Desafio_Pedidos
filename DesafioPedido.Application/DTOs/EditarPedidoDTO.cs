
namespace DesafioPedido.Application.DTOs
{
    public class EditarPedidoDTO
    {
        public int PedidoId { get; set; }

        public int ClienteId { get; set; }

        public List<ItemPedidoDTO> Itens { get; set; } = new();
        public IEnumerable<ClienteDTO> Clientes { get; set; }
        public IEnumerable<ProdutoDto> Produtos { get; set; }
    }
}
