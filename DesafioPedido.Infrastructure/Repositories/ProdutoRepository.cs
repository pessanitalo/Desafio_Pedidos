using Dapper;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Interfaces;
using System.Data;

namespace DesafioPedido.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IDbConnection _connection;

        public ProdutoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateAsync(Produto produto)
        {
            const string sql = @"
                INSERT INTO Produtos (Nome, Descricao, Preco, QuantidadeEstoque)
                VALUES (@Nome,  @Descricao, @Preco, @QuantidadeEstoque)";

            await _connection.ExecuteAsync(sql, produto);
        }


        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Produtos WHERE ProdutoId = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            const string sql = "SELECT * FROM Produtos";
            return await _connection.QueryAsync<Produto>(sql);
        }

        public async Task<IEnumerable<Produto>> GetProdutosDisponiveisAsync()
        {
            const string sql = "SELECT * FROM Produtos WHERE QuantidadeEstoque > 0";
            return await _connection.QueryAsync<Produto>(sql);
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Produtos WHERE ProdutoId = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }

        public async Task UpdateAsync(Produto produto)
        {
            const string sql = @"
                UPDATE Produtos 
                SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque
                WHERE ProdutoId = @ProdutoId";

            await _connection.ExecuteAsync(sql, produto);
        }

        public async Task UpdadeBalanceProductAsync(int produtoId, int quantidade)
        {
            const string sql = @"
            UPDATE Produtos 
            SET QuantidadeEstoque = @quantidade
            WHERE ProdutoId = @ProdutoId";

            await _connection.ExecuteAsync(sql, new { ProdutoId = produtoId, quantidade = quantidade });
        }

        public async Task<bool> VerificarProdutoAoPedido(int id)
        {
            const string sql = @"
                SELECT CASE 
                    WHEN EXISTS (
                        SELECT 1 
                        FROM ItensPedido 
                        WHERE ProdutoId = @Id
                    ) 
                    THEN 1 
                    ELSE 0 
                END";

            var result = await _connection.ExecuteScalarAsync<int>(sql, new { Id = id });

            return result == 1;
        }
    }
}
