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
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await Context.Products.Include(p => p.Category).SingleOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("category/{id}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(int id)
        {
            var products = await Context.Products.Where(p => p.CategoryId == id).ToListAsync();
            if(products == null || products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpPost("range")]
        public async Task<IActionResult> AddRange(List<ViewModels.Product> products)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mapped = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Product, Product>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            ));

            var entities = mapped.Map<List<Product>>(products);

            await Context.Products.AddRangeAsync(entities);
            await Context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Add(ViewModels.Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mapped = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Product, Product>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            ));

            var entity = mapped.Map<Product>(product);

            await Context.Products.AddAsync(entity);
            await Context.SaveChangesAsync();

            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Edit(ViewModels.Product product)
        {
            var entity = await Context.Products.SingleOrDefaultAsync(p => p.Id == product.Id);

            if(entity == null)
            {
                ModelState.AddModelError("Id", "The record id is missing or was entered incorrectly.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Product, Product>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            )).Map(product, entity);

            Context.Update(entity);
            await Context.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await Context.Products.SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                ModelState.AddModelError("Id", "The record id is missing or was entered incorrectly.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Context.Remove(product);
            await Context.SaveChangesAsync();

            return Ok();
        }
    }
}