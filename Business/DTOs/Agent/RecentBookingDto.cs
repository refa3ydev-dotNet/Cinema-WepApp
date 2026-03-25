using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Agent
{
    public class RecentBookingDto
    {
        public string MovieName { get; set; } = string.Empty;
        public string MoviePoster { get; set; } = string.Empty;
        public DateTime ScheduleTime { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string SeatInfo { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
