using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50, MinimumLength = 2)]
        public  string Name { get; set; }
        [Required, StringLength(50, MinimumLength = 2)]
        public string Slug { get; set; }
        [Required, JsonIgnore]
        public List<Product> Products { get; set; }
    }
}
