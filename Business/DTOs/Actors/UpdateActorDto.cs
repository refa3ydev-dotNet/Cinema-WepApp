using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Actors
{
    public class UpdateActorDto
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Bio { get; set; } = string.Empty;

        public IFormFile? ProfilePicture { get; set; }

        public string? ProfilePath { get; set; } // دي هتحط فيها مسار الصورة بعد الرفع
        public string IMDBLink { get; set; } = string.Empty;
        public DateOnly BirthDate { set; get; }
        public DateOnly? DeathDate { get; set; }
        public string Nationality { get; set; } = string.Empty;
    }
}
