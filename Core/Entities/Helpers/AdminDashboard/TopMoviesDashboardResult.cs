using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Admin
{
    public class TopMoviesDashboardResult
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }
        public int TicketsSold { get; set; }
        public decimal Revenue { get; set; }
        public double? Rating { get; set; }
    }
}
