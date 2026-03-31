using DesafioPedido.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioPedido.Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
