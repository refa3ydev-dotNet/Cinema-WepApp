using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Admin
{
    public class AdminDashboardResult
    {

        public DashboardSummaryResult Summary { get; set; }
        public List<ChartPointResult> RevenueChart { get; set; } = new ();
        public List<ChartPointResult> BookingsChart { get; set; } = new ();
        public List<PendingCinemaApprovalResult> PendingCinemas { get; set; } = new();
        public List<TopMoviesDashboardResult> TopMovies { get; set; } = new();
        public List<RecentActivityResult> RecentActivities { get; set; } = new();
    }
}
