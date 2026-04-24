using Business.DTOs.Favorites;

namespace Business.Managers.Favorites
{
    public interface IFavoriteManager
    {
        Task<bool> ToggleFavoriteAsync(string userId, int movieId);
        Task<IEnumerable<FavoriteMovieDto>> GetUserFavoritesAsync(string userId);
    }
}
