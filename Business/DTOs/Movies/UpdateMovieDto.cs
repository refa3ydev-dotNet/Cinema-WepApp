using Core.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Movies
{
    public class UpdateMovieDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Movie Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public decimal Price { get; set; }

        public Language Language { get; set; }
        public TranslationType Translation { get; set; }
        public IFormFile? PosterImg { get; set; }
        public string? PosterUrl { get; set; }
        public IFormFile? BackgroundImg { get; set; }
        public string? BackgroundUrl { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<int> ActorsIds { get; set; } = new List<int>();
        public List<int> ProducerIds { get; set; } = new List<int>();
        public List<int> DirectorIds { get; set; } = new List<int>();
    }
}
