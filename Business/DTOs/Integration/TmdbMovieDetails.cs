using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbMovieDetails:TmdbMovieBasic
    {
        public int Runtime { get; set; } 
        public string Tagline { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public long Budget { get; set; } 
        public long Revenue { get; set; } 

        public string Homepage { get; set; } = string.Empty;
        public string Imdb_Id { get; set; } = string.Empty;

        public TmdbCollection Belongs_To_Collection { get; set; }= new TmdbCollection();

        public List<TmdbProductionsCompany> Production_Companies { get; set; }= new List<TmdbProductionsCompany>();

        public List<TmdbSpokenLanguage> Spoken_Languages { get; set; } = new List<TmdbSpokenLanguage>();

        public List<TmdbGenre> Genres { get; set; } = new List<TmdbGenre>();

        public TmdbCredits Credits { get; set; } = new TmdbCredits();

        public TmdbVideos Videos { get; set; } = new TmdbVideos();
    }
}
