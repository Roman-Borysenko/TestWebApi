using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Contracts
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAll();
        public Task<Product> GetById(int? id);
        public Task<List<Product>> GetByCategory(int id);
        public Task AddRange(List<Product> products);
        public Task Add(Product product);
        public Task Edit(Product product);
        public Task Delete(Product product);
    }
}
