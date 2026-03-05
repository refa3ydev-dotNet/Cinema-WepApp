using Core.Entities;
using Core.Entities.Relations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class Producer : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile Picture is required")]
        public string ProfilePicture { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }
        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date is required")]
        public DateOnly? BirthDate { get; set; }
        public DateOnly? DeathDate { get; set; }
        [Display(Name = "IMDB Link")]
        public string? IMDB { get; set; }
        [Display(Name = "Nationality")]
        [Required(ErrorMessage = "Nationality is required")]
        public string? Nationality { get; set; }
        //Relationships
        public ICollection<ProducerMovie> ProducerMovies { get; set; }=new List<ProducerMovie>();


    }
}
