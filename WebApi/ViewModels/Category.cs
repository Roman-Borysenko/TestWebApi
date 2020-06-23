using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    public class Category
    {
        public int? Id { get; set; }
        [Required, StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
