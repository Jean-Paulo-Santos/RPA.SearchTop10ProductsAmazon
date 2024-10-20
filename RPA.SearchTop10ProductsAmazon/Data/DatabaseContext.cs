using Microsoft.EntityFrameworkCore;
using RPA.SearchTop10ProductsAmazon.Models;

namespace RPA.SearchTop10ProductsAmazon.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        // Construtor que aceita DbContextOptions
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
    }
}
