using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Contracts
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAll();
        public Task<Category> GetById(int? id);
        public Task AddRange(List<Category> categories);
        public Task Add(Category category);
        public Task Edit(Category category);
        public Task Delete(Category category);
    }
}
