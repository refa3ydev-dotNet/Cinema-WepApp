using Core.Entities.Relations;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Director
    {
        public int Id { get; set; }
        [Display(Name = "Director Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { set; get; }
        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Biography { set; get; }
        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile Picture is required")]
        public string? ProfilePicture { get; set; }
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date is required")]
        public DateOnly? BirthDate { get; set; }
        public DateOnly? DeathDate { get; set; }
        [Display(Name = "IMDB Link")]
        public string? IMDB { get; set; }
        [Display(Name = "Nationality")]
        [Required(ErrorMessage = "Nationality is required")]
        public string? Nationality { get; set; }
        // Relations
        public ICollection<DirectorMovie>DirectorMovie { get; set; }=new List<DirectorMovie>();



    }
}
