using Core.Entities;
using Core.Entities.Relations;
using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Movie Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Display(Name = "Movie Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Display(Name = "Movie Price")]
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Display(Name = "Movie Poster")]
        [Required(ErrorMessage = "Poster is required")]
        public string PosterImg { get; set; }
        [Display(Name = "Background Picture")]
        [Required(ErrorMessage = "Background Picture is required")]
        public string BackgroundImg{ get; set; }
        public List<int> CategoriesId { get; set; } = new List<int>(); //many to many>
        [Required(ErrorMessage = "Movie Category is required")]
        [Display(Name = "Movie Category")]
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        [Required(ErrorMessage = "Movie Language is required")]
        [Display(Name = "Movie Language")]
        public Language Language { get; set; }
        [Required(ErrorMessage = "Movie Translation is required")]
        [Display(Name = "Movie Translation")]
        public TranslationType Translation { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        //relations
        public Director director { get; set; }
        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>(); //one to many>
        public ICollection<CinemaMovie>? CinemaMovies { get; set; } = new List<CinemaMovie>(); //many to many>
        public ICollection<MovieSchedule> MovieSchedules { get; set; } = new List<MovieSchedule>();
        [ForeignKey("Producer")]
        public int ProducerId { get; set; }
        public List<Producer> producer { get; set; }

    }
}
