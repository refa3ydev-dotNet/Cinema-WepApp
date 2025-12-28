using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Directors
{
    public class UpdateDirectorDto
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
