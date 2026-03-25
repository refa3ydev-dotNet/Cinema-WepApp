using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
