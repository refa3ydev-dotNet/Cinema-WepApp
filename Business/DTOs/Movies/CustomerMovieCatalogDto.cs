

namespace Business.DTOs.Movies
{
    public class CustomerMovieCatalogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string PosterUrl { get; set; }=string.Empty;
        public string BackgroundImg { get; set; }=string.Empty;
        public string Description { get; set; }=string.Empty;
        public int Runtime { get; set; }
        public double Rating { get; set; }
        public List<string>? CategoryNames { get; set; }
        public bool HasActiveSchedules { get; set; }
    }
}
