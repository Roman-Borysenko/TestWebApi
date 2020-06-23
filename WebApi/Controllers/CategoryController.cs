using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        public async Task<Category> GetOne(int id)
        {
            return await Context.Categories.SingleOrDefaultAsync(c => c.Id == id);
        }
        [HttpPost("range")]
        public async Task<int> AddRange(List<ViewModels.Category> categories)
        {
            var mapped = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Category, Category>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            ));
            var entities = mapped.Map<List<Category>>(categories);

            await Context.Categories.AddRangeAsync(entities);
            await Context.SaveChangesAsync();

            return categories.Count;
        }
        [HttpPost]
        public async Task<bool> Add(ViewModels.Category category)
        {
            var mapped = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Category, Category>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            ));
            var entity = mapped.Map<Category>(category);

            await Context.Categories.AddAsync(entity);
            await Context.SaveChangesAsync();

            return true;
        }
        [HttpPut]
        public async Task<bool> Edit(ViewModels.Category category)
        {
            var entity = await Context.Categories.SingleOrDefaultAsync(c => c.Id == category.Id);
            new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<ViewModels.Category, Category>()
                .ForMember("Slug", opt => opt.MapFrom(src => src.Name.GenerateSlug("-")))
            )).Map<ViewModels.Category, Category>(category, entity);

            Context.Update(entity);
            await Context.SaveChangesAsync();

            return true;
        }
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            Context.Categories.Remove(await Context.Categories.SingleOrDefaultAsync(c => c.Id == id));
            await Context.SaveChangesAsync();

            return true;
        }
    }
}