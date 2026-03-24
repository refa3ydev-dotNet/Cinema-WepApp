using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbCollection
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Poster_Path { get; set; } = string.Empty;
        public string Backdrop_Path { get; set; } = string.Empty;
    }
}
