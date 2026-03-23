using Core.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Movies
{
    public class CreateMovieDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Movie Name is required")]
        public string Name { get; set; }=string.Empty;
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }=string.Empty;
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        public IFormFile? PosterImg { get; set; }
        public string? PosterUrl { get; set; }
        public IFormFile? BackgroundImg { get; set; } //BackgroundImg
        public string? BackgroundUrl { get; set; }
        public Language Language { get; set; }
        public TranslationType Translation { get; set; }
        [Required(ErrorMessage = "Please select at least one Actor")]
        public List<int> ActorsIds { get; set; }=new List<int>();
        [Required(ErrorMessage = "Please select at least one Producer")]
        public List<int> ProducerIds { get; set; }=new List<int>();
        [Required(ErrorMessage = "Please select at least one Category")]
        public List<int> CategoryIds { get; set; }= new List<int>();
        [Required(ErrorMessage = "Please select at least one Director")]
        public List<int> DirectorIds { get; set; }= new List<int>();
        

    }
}
