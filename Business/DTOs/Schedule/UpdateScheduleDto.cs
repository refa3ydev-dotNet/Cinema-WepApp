using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.DTOs.Schedule
{
    public class UpdateScheduleDto
    {
        public  int Id { get; set; }
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        [Required(ErrorMessage = "please select a room")]
        [Display(Name = "Room")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "please select a start time")]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "please select a price")]
        [Range(0, 10000, ErrorMessage = "Price must between 0 and 10000")]
        public decimal Price { get; set; }
        public int RunTime { get; set; }
    }
}
