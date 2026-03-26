using Core.Entities.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class Booking : BaseEntity
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }=DateTime.Now;
        [Required]
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }="Confirmed";
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int MovieScheduleId { get; set; }
        public MovieSchedule MovieSchedule { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; } = new HashSet<BookingSeat>();
    }
}
