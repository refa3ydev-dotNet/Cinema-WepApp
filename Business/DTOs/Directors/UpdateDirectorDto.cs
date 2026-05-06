using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Directors
{
    public class UpdateDirectorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Director name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Biography is required")]
        public string Biography { get; set; } = string.Empty;

        [Url(ErrorMessage = "Please enter a valid image URL")]
        public string? ProfilePictureUrl { get; set; }

        public IFormFile? ProfilePicture { get; set; }
        public DateOnly? BirthDate { get; set; }

        [Url(ErrorMessage = "Please enter a valid IMDb URL")]
        public string? IMDB { get; set; }

        public DateOnly? DeathDate { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        public string Nationality { get; set; } = string.Empty;
    }
}
