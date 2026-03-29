using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Movies
{
    public class TmdbMovieDetailsDto
    {
        public int TmdbId { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; }= string.Empty;
        public int Runtime { get; set; }
        public string PosterPath { get; set; }=string.Empty;
        public string BackdropPath { get; set; } =string.Empty;
    }
}
