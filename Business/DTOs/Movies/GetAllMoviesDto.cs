using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Movies
{
    public class GetAllMoviesDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile PosterImg { get; set; }
        public string PosterUrl { get; set; }
        public IFormFile BackgroundImg { get; set; } //BackgroundImg
        public string BackgroundUrl { get; set; }
        public List<string> CategoryNames { get; set; }
        public Language Language { get; set; }
        public TranslationType Translation { get; set; }
        public List<string> Cinemas { get; set; }
        public List<string> Actors { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
