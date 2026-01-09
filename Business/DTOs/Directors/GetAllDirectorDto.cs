using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Directors
{
    public class GetAllDirectorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? IMDB { get; set; }
    }
}
