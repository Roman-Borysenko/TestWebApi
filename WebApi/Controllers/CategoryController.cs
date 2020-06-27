using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlugGenerator;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public DataContext Context;
        public CategoryController(DataContext context)
        {
            Context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAll()
        {
            return await Context.Categories.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var category = await Context.Categories.SingleOrDefaultAsync(c => c.Id == id);
            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost("range")]
        public async Task<IActionResult> AddRange(List<ViewModels.Category> categories)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mapped = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Category, Category>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            ));
            var entities = mapped.Map<List<Category>>(categories);

            await Context.Categories.AddRangeAsync(entities);
            await Context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Add(ViewModels.Category category)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mapped = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Category, Category>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            ));
            var entity = mapped.Map<Category>(category);

            await Context.Categories.AddAsync(entity);
            await Context.SaveChangesAsync();

            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Edit(ViewModels.Category category)
        {
            var entity = await Context.Categories.SingleOrDefaultAsync(c => c.Id == category.Id);

            if(entity == null)
            {
                ModelState.AddModelError("Id", "The record id is missing or was entered incorrectly.");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Category, Category>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            )).Map<ViewModels.Category, Category>(category, entity);

            Context.Update(entity);
            await Context.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await Context.Categories.SingleOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                ModelState.AddModelError("Id", "The record id is missing or was entered incorrectly.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Context.Categories.Remove(category);
            await Context.SaveChangesAsync();

            return Ok();
        }
    }
}