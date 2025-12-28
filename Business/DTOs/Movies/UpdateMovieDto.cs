using Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Movies
{
    public class UpdateMovieDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile PosterImg { get; set; }
        public string PosterUrl { get; set; }
        public List<int> CategoryIds { get; set; }
        public Language Language { get; set; }
        public TranslationType Translation { get; set; }
        public IFormFile BackgroundImg { get; set; } //BackgroundImg
        public string BackgroundUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> CategoryName { get; set; }

        //public string? cinemaName { get; set; }
        public List<int>? CinemasIds { get; set; }
        public List<int> ActorsIds { get; set; }
        public int producerId { get; set; }
    }
}
