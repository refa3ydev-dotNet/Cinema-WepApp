using Core.Entities.Relations;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Profile Picture")]
        public string? ProfilePicture { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 50 characters")]
        public string FullName { get; set; }
        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date is required")]
        public DateOnly? BirthDate { get; set; }
        public DateOnly? DeathDate { get; set; }
        [Display(Name = "IMDB Link")]
        public string? IMDBLink { get; set; }
        [Display(Name = "Nationality")]
        [Required(ErrorMessage = "Nationality is required")]
        public string? Nationality { get; set; }

        //relationship
        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

    }
}
