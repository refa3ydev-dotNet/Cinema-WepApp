using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Business.DTOs.Integration
{
    public class TmdbMovieBasic
    {
        public int Id { get; set; }
        public string Title { get; set; }=string.Empty;
        public string Overview { get; set; } = string.Empty;
        [JsonPropertyName("poster_path")]
        public string Poster_Path { get; set; } = string.Empty;
        [JsonPropertyName("backdrop_path")]
        public string Backdrop_Path { get; set; } = string.Empty;
        [JsonPropertyName("release_date")]
        public string Release_Date { get; set; } = string.Empty;
        [JsonPropertyName("vote_average")]
        public double Vote_Average { get; set; }
        [JsonPropertyName("vote_count")]
        public int Vote_Count { get; set; } // عدد الناس اللي قيموا الفيلم
        [JsonPropertyName("original_language")]
        public string Original_Language { get; set; } = string.Empty;
        public bool Adult { get; set; } // تصنيف عمري (للكبار فقط؟)
        [JsonPropertyName("genre_ids")]
        public List<int> Genre_Ids { get; set; } = new List<int>();
    }
}
