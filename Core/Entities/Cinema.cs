using Core.Entities;
using Core.Entities.Relations;
using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Cinema Name")]
        [Required(ErrorMessage = "Cinema Name is required")]
        public string Name { get; set; }
        [Display(Name = "Cinema Logo")]
        [Required(ErrorMessage = "Cinema Logo is required")]
        public string? Logo { get; set; }
        [Display(Name = "Cinema Description")]
        [Required(ErrorMessage = "Cinema Description is required")]
        public string Description { get; set; }
        [Display(Name = "Cinema Address")]
        [Required(ErrorMessage = "Cinema Address is required")]
        public string Address { get; set; }
        [Display(Name = "Cinema Background Picture")]
        [Required(ErrorMessage = "Cinema Background Picture is required")]
        public string? BackgroundPicture { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }= ApprovalStatus.Pending;

        //Relationships
        public ICollection<CinemaMovie> CinemaMovies { get; set; } = new List<CinemaMovie>();
        public ICollection<MovieSchedule> movieSchedules { get; set; } = new List<MovieSchedule>();
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}
