namespace Business.DTOs.Favorites
{
    public class FavoriteMovieDto
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public DateTime AddedDate { get; set; }
    }
}
