using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Actors
{
    public class CreateActorDto
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Profile Picture is required")]
        public IFormFile ProfilePicture { get; set; }
        public string IMDBLink { get; set; }
        public string? ProfilePath { get; set; } // دي هتحط فيها مسار الصورة بعد الرفع
    }
}
