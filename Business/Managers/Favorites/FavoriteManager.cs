using Business.DTOs.Favorites;
using Business.Mapping;
using DataAccess.Repositories.Favorite;

namespace Business.Managers.Favorites
{
    public class FavoriteManager : IFavoriteManager
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteManager(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<bool> ToggleFavoriteAsync(string userId, int movieId)
        {
            await _favoriteRepository.ToggleFavoriteAsync(userId, movieId);
            return _favoriteRepository.IsFavorite(userId, movieId);
        }

        public async Task<IEnumerable<FavoriteMovieDto>> GetUserFavoritesAsync(string userId)
        {
            var favoriteMovies = await _favoriteRepository.GetUserFavoritesAsync(userId);
            return favoriteMovies.ToFavoriteMovieDto();
        }
    }
}
