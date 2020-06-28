using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlugGenerator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public IProductRepository Repository { get; set; }
        public ProductController(IProductRepository repository)
        {
            Repository = repository;
        }
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await Repository.GetAll();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await Repository.GetById(id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("category/{id}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(int id)
        {
            var products = await Repository.GetByCategory(id);
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

            await Repository.AddRange(entities);

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

            await Repository.Add(entity);

            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Edit(ViewModels.Product product)
        {
            var entity = await Repository.GetById(product.Id);

            if(entity == null)
            {
                ModelState.AddModelError("Id", "The record id is missing or was entered incorrectly.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Product, Product>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            )).Map(product, entity);

            await Repository.Edit(entity);

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await Repository.GetById(id);

            if (product == null)
            {
                ModelState.AddModelError("Id", "The record id is missing or was entered incorrectly.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await Repository.Delete(product);

            return Ok();
        }
    }
}