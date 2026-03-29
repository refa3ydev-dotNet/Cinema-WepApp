using Business.DTOs.Actors;
using Business.DTOs.Cinemas;
using Business.DTOs.Directors;
using Business.DTOs.Producers;
using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Movies
{
    public class GetMovieByIdDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? PosterUrl { get; set; }
        public string? BackgroundUrl { get; set; }
        public List<string> CategoryName { get; set; }
        public Language Language { get; set; }
        public TranslationType Translation { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Runtime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        //public string? cinemaName { get; set; }
        //public DateTime CreatedDate { get; set; }
        public List<int>? CinemasIds { get; set; }=new List<int>();
        public List<int> ActorsIds { get; set; } = new List<int>();
        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<int> ProducersIds { get; set; }= new List<int>();
        public List<int> DirectorsIds { get; set; } = new List<int>();

        public List<DirectorMoviesDto> Directors { get; set; }
        public List<ProducerInMovieDto> Producers { get; set; }
        public List<CinemaMoviesDto> Cinemas { get; set; }
        public List<ActorsInMovieDto> Actors { get; set; } = new List<ActorsInMovieDto>();



    }
}
