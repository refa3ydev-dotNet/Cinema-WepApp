using Core.Entities.Relations;

namespace Core.Entities
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Biography { set; get; }
        public string? ProfilePicture { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? IMDB { get; set; }
        // Relations
        public ICollection<DirectorMovie>DirectorMovie { get; set; }=new List<DirectorMovie>();



    }
}
