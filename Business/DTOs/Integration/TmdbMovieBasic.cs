using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Integration
{
    public class TmdbMovieBasic
    {
        public int Id { get; set; }
        public string Title { get; set; }=string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string Poster_Path { get; set; } = string.Empty;
        public string Backdrop_Path { get; set; } = string.Empty;
        public string Release_Date { get; set; } = string.Empty;
        public double Vote_Average { get; set; }
        public int Vote_Count { get; set; } // عدد الناس اللي قيموا الفيلم
        public string Original_Language { get; set; } = string.Empty;
        public bool Adult { get; set; } // تصنيف عمري (للكبار فقط؟)

    }
}
