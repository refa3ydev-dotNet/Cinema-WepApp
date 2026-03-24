using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbSearchResponse
    {
        public int Page { get; set; } 
        public List<TmdbMovieBasic> Results { get; set; } = new List<TmdbMovieBasic>();
        public int Total_Pages { get; set; }
        public int Total_Results { get; set; } 
    }
}
