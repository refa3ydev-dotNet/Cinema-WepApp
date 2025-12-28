using Core;
using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Movies
{
    public class CreateMovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile PosterImg { get; set; }
        public string? PosterUrl { get; set; }
        public IFormFile BackgroundImg { get; set; } //BackgroundImg
        public string? BackgroundUrl { get; set; }
        public List<int> CategoryIds { get; set; }
        public Language Language { get; set; }
        public TranslationType Translation { get; set; }
        public List<int>? CinemasIds { get; set; }
        public List<int> ActorsIds { get; set; }
        public int producerId { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
