using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Directors
{
    public class GetDirectorByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? IMDB { get; set; }
        public DateOnly? DeathDate { get; set; }
        public string Nationality { get; set; } = string.Empty;
    }
}
