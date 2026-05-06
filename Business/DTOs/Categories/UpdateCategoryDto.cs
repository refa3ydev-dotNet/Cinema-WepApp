using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        [Url(ErrorMessage = "Please enter a valid image URL")]
        public string? ImageUrl { get; set; }
    }
}
