using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Producers
{
    public class UpdateProducerDto
    {
        public int Id { get; set; }
        public string? ProfilePath { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string IMDBLink { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateOnly? DeathDate { get; set; }
        public string Nationality { get; set; }


    }
}
