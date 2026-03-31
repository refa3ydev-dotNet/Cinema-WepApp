using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Schedule
{
    public class ScheduleDisplayDto
    {
        public int Id { get; set; }
        public string MovieName { get; set; }=string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }=string.Empty;
    }
}
