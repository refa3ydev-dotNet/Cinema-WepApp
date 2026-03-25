using Business.DTOs.Movies;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Actors
{
    public class GetActorByIdDto
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Bio { get; set; } = string.Empty;

        public IFormFile? ProfilePicture { get; set; }
        public string? ProfilePath { get; set; } // دي هتحط فيها مسار الصورة بعد الرفع
        public string IMDBLink { get; set; } = string.Empty;
        public List<GetAllMoviesDto> MovieDetails { get; set; } = new List<GetAllMoviesDto>();
        public DateOnly BirthDate {  get; set; }
        public DateOnly? DeathDate { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public int? Age
        {
            get
            {
                var endDate=DeathDate??DateOnly.FromDateTime(DateTime.Now);
                var age= endDate.Year - BirthDate.Year;
                if (BirthDate > endDate.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
