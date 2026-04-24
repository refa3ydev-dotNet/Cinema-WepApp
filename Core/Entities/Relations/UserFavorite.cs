using Core.Entities;

namespace Core.Entities.Relations
{
    public class UserFavorite
    {
        public string UserId { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;

        public ApplicationUser User { get; set; } = null!;
        public Movie Movie { get; set; } = null!;
    }
}
