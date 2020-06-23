using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    }
}