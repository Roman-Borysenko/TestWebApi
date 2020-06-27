using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlugGenerator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public DataContext Context { get; set; }
        public ProductController(DataContext context)
        {
            Context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await Context.Products.Include(p => p.Category).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<Product> GetProduct(int id)
        {
            return await Context.Products.Include(p => p.Category).SingleOrDefaultAsync(p => p.Id == id);
        }
        [HttpGet("category/{id}")]
        public async Task<List<Product>> GetProductsByCategory(int id)
        {
            return await Context.Products.Where(p => p.CategoryId == id).ToListAsync();
        }
        [HttpPost("range")]
        public async Task<int> AddRange(List<ViewModels.Product> products)
        {
            var mapped = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Product, Product>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            ));

            var entities = mapped.Map<List<Product>>(products);

            await Context.Products.AddRangeAsync(entities);
            await Context.SaveChangesAsync();

            return products.Count;
        }
        [HttpPost]
        public async Task<bool> Add(ViewModels.Product product)
        {
            var mapped = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Product, Product>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            ));

            var entity = mapped.Map<Product>(product);

            await Context.Products.AddAsync(entity);
            await Context.SaveChangesAsync();

            return true;
        }
        [HttpPut]
        public async Task<bool> Edit(ViewModels.Product product)
        {
            var entity = await Context.Products.SingleOrDefaultAsync(p => p.Id == product.Id);

            new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Product, Product>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            )).Map(product, entity);

            Context.Update(entity);
            await Context.SaveChangesAsync();

            return true;
        }
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            Context.Remove(await Context.Products.SingleOrDefaultAsync(p => p.Id == id));
            await Context.SaveChangesAsync();

            return true;
        }
    }
}