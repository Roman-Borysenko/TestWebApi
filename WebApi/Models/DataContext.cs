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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Category #1", Slug = "category-#1" },
                new Category() { Id = 2, Name = "Category #2", Slug = "category-#2" },
                new Category() { Id = 3, Name = "Category #3", Slug = "category-#3" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Product #1", Slug = "product-#1", CategoryId = 1, Description = "Product #1 Description", Price = 11, Quantity = 111 },    
                new Product() { Id = 2, Name = "Product #2", Slug = "product-#2", CategoryId = 2, Description = "Product #2 Description", Price = 22, Quantity = 222 },    
                new Product() { Id = 3, Name = "Product #3", Slug = "product-#3", CategoryId = 3, Description = "Product #3 Description", Price = 33, Quantity = 333 },    
                new Product() { Id = 4, Name = "Product #4", Slug = "product-#4", CategoryId = 1, Description = "Product #4 Description", Price = 44, Quantity = 444 }    
            );
        }
    }
}
