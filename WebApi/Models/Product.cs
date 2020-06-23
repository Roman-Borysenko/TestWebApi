using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required, StringLength(50, MinimumLength = 2)]
        public string Slug { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required, StringLength(1000, MinimumLength = 2)]
        public string Description { get; set; }
        [Required, Range(1, 10000)]
        public int Price { get; set; }
        [Required, Range(1, 1000000)]
        public int Quantity { get; set; }
    }
}
