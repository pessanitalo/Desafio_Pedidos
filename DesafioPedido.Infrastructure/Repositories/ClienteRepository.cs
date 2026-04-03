using Dapper;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Interfaces;
using System.Data;

namespace DesafioPedido.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {

        private readonly IDbConnection _connection;

        public ClienteRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateAsync(Cliente cliente)
        {
            const string sql = @"
                INSERT INTO Clientes (Nome, Email, Telefone, DataCadastro)
                VALUES (@Nome, @Email, @Telefone, @DataCadastro)";

            await _connection.ExecuteAsync(sql, cliente);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Clientes WHERE ClienteId = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync(string? nome, string? email)
        {
            const string sql = @"
                SELECT * FROM Clientes
                WHERE (@Nome IS NULL OR Nome LIKE @Nome)
                  AND (@Email IS NULL OR Email LIKE @Email)
                ORDER BY Nome";

            return await _connection.QueryAsync<Cliente>(sql, new
            {
                Nome = string.IsNullOrWhiteSpace(nome) ? null : $"%{nome}%",
                Email = string.IsNullOrWhiteSpace(email) ? null : $"%{email}%"
            });
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM Clientes WHERE ClienteId = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            const string sql = @"
                     UPDATE Clientes 
                     SET Nome = @Nome, Email = @Email, Telefone = @Telefone, DataCadastro = @DataCadastro
                    WHERE ClienteId = @ClienteId";

            await _connection.ExecuteAsync(sql, cliente);
        }
    }
}
