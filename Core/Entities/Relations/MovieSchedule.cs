using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Relations
{
    public class MovieSchedule
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }
        public int? RoomId { get; set; }
        public Room? Room { get; set; }

    }
}
