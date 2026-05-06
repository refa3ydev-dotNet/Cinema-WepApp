using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Producers
{
    public class UpdateProducerDto
    {
        public int Id { get; set; }
        public string? ProfilePath { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        [Required(ErrorMessage = "Producer name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bio is required")]
        public string Bio { get; set; } = string.Empty;

        [Url(ErrorMessage = "Please enter a valid IMDb URL")]
        public string? IMDBLink { get; set; }

        public DateOnly? BirthDate { get; set; }
        public DateOnly? DeathDate { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        public string Nationality { get; set; } = string.Empty;
    }
}
