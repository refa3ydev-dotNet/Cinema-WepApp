using Core.Entities;
using Core.Entities.Relations;
using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class Movie : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Movie Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }=string.Empty;
        [Display(Name = "Movie Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
        [Display(Name = "Movie Price")]
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Display(Name = "Movie Poster")]
        [Required(ErrorMessage = "Poster is required")]
        public string PosterImg { get; set; } = string.Empty;
        [Display(Name = "Background Picture")]
        [Required(ErrorMessage = "Background Picture is required")]
        public string BackgroundImg { get; set; } = string.Empty;
        [NotMapped]
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
        public int TmdbId { get; set; }
        public string TrailerUrl { get; set; }=string.Empty;
        [Display(Name = "Movie Runtime (Minutes)")]
        public int Runtime { get; set; }
        public decimal Rating { get; set; }
        public string ReleaseDate { get; set; } = string.Empty;

        //public DateTime CreatedDate { get; set; } = DateTime.Now;

        //relations
        public ICollection<DirectorMovie> DirectorMovies { get; set; }= new List<DirectorMovie>();
        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>(); //one to many>
        public ICollection<CinemaMovie> CinemaMovies { get; set; } = new List<CinemaMovie>(); //many to many>
        public ICollection<MovieSchedule> MovieSchedules { get; set; } = new List<MovieSchedule>();
        public ICollection<ProducerMovie> ProducerMovies { get; set; }=new List<ProducerMovie>();
        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();

    }
}
