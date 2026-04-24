using Core;
using Core.Entities.Relations;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Favorite
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly MoviesDbContext _context;

        public FavoriteRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public bool IsFavorite(string userId, int movieId)
        {
            return _context.UserFavorites.Any(favorite => favorite.UserId == userId && favorite.MovieId == movieId);
        }

        public async Task ToggleFavoriteAsync(string userId, int movieId)
        {
            var existingFavorite = await _context.UserFavorites.FindAsync(userId, movieId);

            if (existingFavorite is not null)
            {
                _context.UserFavorites.Remove(existingFavorite);
            }
            else
            {
                var favorite = new UserFavorite
                {
                    UserId = userId,
                    MovieId = movieId,
                    AddedDate = DateTime.Now
                };

                await _context.UserFavorites.AddAsync(favorite);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> GetUserFavoritesAsync(string userId)
        {
            var favorites = await _context.UserFavorites
                .Include(favorite => favorite.Movie)
                .Where(favorite => favorite.UserId == userId)
                .OrderByDescending(favorite => favorite.AddedDate)
                .ToListAsync();

            return favorites.Select(favorite =>
            {
                favorite.Movie.UserFavorites = new List<UserFavorite> { favorite };
                return favorite.Movie;
            });
        }
    }
}
