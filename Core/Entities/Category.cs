using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Category Name is required")]
        public string CategoryName { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Display(Name = "Image URL")]
        [Required(ErrorMessage = "Image URL is required")]
        public string? ImageUrl { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>(); // navigation property>
    }
}
