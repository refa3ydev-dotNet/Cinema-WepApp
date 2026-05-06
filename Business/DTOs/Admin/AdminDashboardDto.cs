using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Admin
{
    public class AdminDashboardDto
    {

        public DashboardSummaryDto Summary { get; set; }
        public List<ChartPointDto> RevenueChart { get; set; } = new ();
        public List<ChartPointDto> BookingsChart { get; set; } = new ();
        public List<PendingCinemaApprovalDto> PendingCinemas { get; set; } = new();
        public List<TopMoviesDashboardDto> TopMovies { get; set; } = new();
        public List<RecentActivityDto> RecentActivities { get; set; } = new();
    }
}
