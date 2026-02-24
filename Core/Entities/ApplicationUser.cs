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
        // الخاصية دي هتكون true أوتوماتيك للعميل العادي، وهتكون false للوكيل لحد ما المدير يوافق
        public bool IsApproved { get; set; } = true;
        public string Gender { get; set; }

        //public ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();
    }
}
