using Core;

namespace DataAccess.Repositories.Favorite
{
    public interface IFavoriteRepository
    {
        bool IsFavorite(string userId, int movieId);
        Task ToggleFavoriteAsync(string userId, int movieId);
        Task<IEnumerable<Movie>> GetUserFavoritesAsync(string userId);
    }
}
