using Core.Entities.Relations;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Room name is required")]
        [Display(Name = "Room name")]
        public string RoomName { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<MovieSchedule> MovieSchedules { get; set; } = new List<MovieSchedule>(); // <MovieSchedule>
    }
}
