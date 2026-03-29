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
        public IFormFile? PosterImg { get; set; }
        public string PosterUrl { get; set; }
        public IFormFile? BackgroundImg { get; set; } //BackgroundImg
        public string BackgroundUrl { get; set; }
        public Language Language { get; set; }
        public TranslationType Translation { get; set; }
        public decimal Rating { get; set; } = 0;
        public string Release_Date { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Runtime { get; set; }
        public List<string> CategoryNames { get; set; } = new List<string>();
        public List<string> Cinemas { get; set; } = new List<string>();
        public List<string> Actors { get; set; } = new List<string>();
        public List<string> Directors { get; set; } = new List<string>();
        public List<string> Producers { get; set; } = new List<string>();
    }
}
