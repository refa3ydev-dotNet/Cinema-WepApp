using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Producers
{
    public class CreateProducerDto
    {
        public int Id { get; set; }
        public string? ProfilePath { get; set; }
        [Required(ErrorMessage = "Please upload a profile picture.")]
        public IFormFile ProfilePicture { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
         public string IMDBLink { get; set; }
    }
}
