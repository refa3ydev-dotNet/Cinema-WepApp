using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Agent
{
    public class RecentBookingDto
    {
        public string MovieName { get; set; }
        public string MoviePoster { get; set; }
        public DateTime ScheduleTime { get; set; }
        public string RoomName { get; set; }
        public string CustomerName { get; set; }
        public string SeatInfo { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
