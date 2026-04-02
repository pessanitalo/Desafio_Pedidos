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

        public async Task UpdateAsync(Pedido pedido, List<ItemPedido> itens)
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();

            try
            {
                const string updatePedido = @"
                    UPDATE Pedidos
                    SET ClienteId = @ClienteId,
                        DataPedido = @DataPedido,
                        ValorTotal = @ValorTotal,
                        Status = @Status
                    WHERE PedidoId = @PedidoId";

                await _connection.ExecuteAsync(updatePedido, pedido, transaction);

                const string deleteItens = @"
                DELETE FROM ItensPedido
                WHERE PedidoId = @PedidoId";

                await _connection.ExecuteAsync(deleteItens, new { pedido.PedidoId }, transaction);

                const string insertItem = @"
                    INSERT INTO ItensPedido (PedidoId, ProdutoId, Quantidade, PrecoUnitario)
                    VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario)";

                foreach (var item in itens)
                {
                    item.PedidoId = pedido.PedidoId;
                    await _connection.ExecuteAsync(insertItem, item, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
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
            // FIX: incluído p.ClienteId para pré-selecionar o cliente na tela de edição
            const string sqlPedido = @"
                SELECT p.PedidoId, p.ClienteId, c.Nome AS NomeCliente, p.DataPedido, p.ValorTotal, p.Status
                FROM Pedidos p
                INNER JOIN Clientes c ON c.ClienteId = p.ClienteId
                WHERE p.PedidoId = @Id";

            // FIX: incluído i.ProdutoId para pré-selecionar o produto na tela de edição
            const string sqlItens = @"
                SELECT i.ProdutoId, pr.Nome AS NomeProduto, i.Quantidade, i.PrecoUnitario
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
