using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
