using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private DataContext Context { get; set; }
        public ProductRepository(DataContext context)
        {
            Context = context;
        }
        public async Task Add(Product product)
        {
            await Context.Products.AddAsync(product);
            await Context.SaveChangesAsync();
        }

        public async Task AddRange(List<Product> products)
        {
            await Context.Products.AddRangeAsync(products);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            Context.Remove(product);
            await Context.SaveChangesAsync();
        }

        public async Task Edit(Product product)
        {
            Context.Update(product);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAll()
        {
            return await Context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<List<Product>> GetByCategory(int id)
        {
            return await Context.Products.Where(p => p.CategoryId == id).ToListAsync();
        }

        public async Task<Product> GetById(int? id)
        {
            return await Context.Products.Include(p => p.Category).SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
