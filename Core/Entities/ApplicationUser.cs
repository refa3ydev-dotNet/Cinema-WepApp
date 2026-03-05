using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public int?CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public bool IsApproved { get; set; } = true;
        public string Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now; 
        public DateTime? UpdateAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteAt { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
