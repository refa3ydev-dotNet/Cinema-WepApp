using Business.DTOs.Movies;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Cinemas
{
    public class GetCinemaByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public IFormFile BackgroundPicture { get; set; }
        public string? BackgroundPath { get; set; }
        public string? LogoPath { get; set; }
        public IFormFile Logo { get; set; }
        public List<GetAllMoviesDto> MovieDetails { get; set; }


    }
}
