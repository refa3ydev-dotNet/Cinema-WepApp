using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }=new Movie();
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema Cinema { get; set; }=new Cinema();
        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room? Room { get; set; }

    }
}
