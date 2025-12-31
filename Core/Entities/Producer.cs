using Core.Entities.Relations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class Producer
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Profile Picture")]

        public string ProfilePicture { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }
        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }
        [Display(Name = "IMDB")]
        [Required(ErrorMessage = "IMDB is required")]
        public string IMDB { get; set; }
        //Relationships
        public ICollection<ProducerMovie> ProducerMovies { get; set; }=new List<ProducerMovie>();


    }
}
