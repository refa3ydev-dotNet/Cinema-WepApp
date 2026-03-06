using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Agent
{
    public class AgentDashboardDto
    {
        public string AgentName { get; set; }
        public string CinemaName { get; set; }
        public int TicketSoldToday { get; set; }
        public decimal DailyRevenue { get; set; }
        public int ActiveMoviesCount { get; set; }
        public List<RecentBookingDto> RecentBookings { get; set; }=new List<RecentBookingDto>();
    }
}
