using Dapper;
using DesafioPedido.Domain.DTOs;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Interfaces;
using System.Data;

namespace DesafioPedido.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IDbConnection _connection;

        public PedidoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AddItemAsync(ItemPedido item)
        {
            const string sql = @"
            INSERT INTO ItensPedido (PedidoId, ProdutoId, Quantidade, PrecoUnitario)
            VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario)";

            await _connection.ExecuteAsync(sql, item);
        }

        public async Task DeleteAsync(int id)
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();

            try
            {
                const string deleteItens = "DELETE FROM ItensPedido WHERE PedidoId = @Id";
                await _connection.ExecuteAsync(deleteItens, new { Id = id }, transaction);

                const string deletePedido = "DELETE FROM Pedidos WHERE PedidoId = @Id";
                await _connection.ExecuteAsync(deletePedido, new { Id = id }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<PedidoResumoDTO>> GetAllAsync(int? clienteId, string? status)
        {
            var sql = @"
                    SELECT p.PedidoId, c.Nome AS NomeCliente, p.DataPedido, p.ValorTotal, p.Status
                    FROM Pedidos p
                    INNER JOIN Clientes c ON c.ClienteId = p.ClienteId
                    WHERE (@ClienteId IS NULL OR p.ClienteId = @ClienteId)
                      AND (@Status    IS NULL OR p.Status    = @Status)
                    ORDER BY p.DataPedido DESC";

            return await _connection.QueryAsync<PedidoResumoDTO>(sql, new { ClienteId = clienteId, Status = status });
        }

        public Task UpdateAsync(Pedido cliente)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTotalAsync(int pedidoId, decimal valorTotal)
        {
            const string sql = @"
            UPDATE Pedidos SET ValorTotal = @ValorTotal 
            WHERE PedidoId = @PedidoId";

            await _connection.ExecuteAsync(sql, new { PedidoId = pedidoId, ValorTotal = valorTotal });
        }

        public async Task<int> CreateAsync(Pedido pedido)
        {
            const string sql = @"
            INSERT INTO Pedidos (ClienteId, DataPedido, ValorTotal, Status)
            VALUES (@ClienteId, @DataPedido, @ValorTotal, @Status);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await _connection.ExecuteScalarAsync<int>(sql, pedido);
        }

        public async Task<PedidoDetalhesDTO> GetByIdAsync(int id)
        {
            const string sqlPedido = @"
                SELECT p.PedidoId, c.Nome AS NomeCliente, p.DataPedido, p.ValorTotal, p.Status
                FROM Pedidos p
                INNER JOIN Clientes c ON c.ClienteId = p.ClienteId
                WHERE p.PedidoId = @Id";

            const string sqlItens = @"
                SELECT pr.Nome AS NomeProduto, i.Quantidade, i.PrecoUnitario
                FROM ItensPedido i
                INNER JOIN Produtos pr ON pr.ProdutoId = i.ProdutoId
                WHERE i.PedidoId = @Id";

            var pedido = await _connection.QueryFirstOrDefaultAsync<PedidoDetalhesDTO>(sqlPedido, new { Id = id });

            if (pedido is null)
                return null;

            pedido.Itens = (await _connection.QueryAsync<ItemPedidoDetalhesDTO>(sqlItens, new { Id = id })).ToList();

            return pedido;
        }
    }
}
