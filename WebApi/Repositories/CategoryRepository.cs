using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public DataContext Context { get; set; }
        public CategoryRepository(DataContext context)
        {
            Context = context;
        }

        public async Task Add(Category category)
        {
            await Context.Categories.AddAsync(category);
            await Context.SaveChangesAsync();
        }

        public async Task AddRange(List<Category> categories)
        {
            await Context.Categories.AddRangeAsync(categories);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            Context.Categories.Remove(category);
            await Context.SaveChangesAsync();
        }

        public async Task Edit(Category category)
        {
            Context.Update(category);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAll()
        {
            return await Context.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int? id)
        {
            return await Context.Categories.SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
