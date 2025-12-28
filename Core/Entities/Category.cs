namespace Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>(); // navigation property>
    }
}
