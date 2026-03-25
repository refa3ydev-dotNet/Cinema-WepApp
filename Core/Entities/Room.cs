using Core.Entities.Relations;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Room : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Room name is required")]
        [Display(Name = "Room name")]
        public string RoomName { get; set; }=string.Empty;
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }=new Cinema();
        public int SeatCount { get; set; }
        public int SeatsPerRow { get; set; }
        public ICollection<MovieSchedule> MovieSchedules { get; set; } = new List<MovieSchedule>(); // <MovieSchedule>
        public ICollection<Seat>Seats { get; set; } = new List<Seat>();
    }
}
