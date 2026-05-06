using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Admin
{
    public class DashboardSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalBookings { get; set; }
        public int ActiveCinemas { get; set; }
        public int PendingApprovals { get; set; }
        public int RegisteredUsers { get; set; }

        public decimal RevenueGrowthPercentage { get; set; }
        public decimal BookingGrowthPercentage { get; set; }
    }
}
