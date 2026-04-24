using Business.DTOs.Favorites;
using Core;

namespace Business.Mapping
{
    public static class FavoriteMapping
    {
        public static IEnumerable<FavoriteMovieDto> ToFavoriteMovieDto(this IEnumerable<Movie> movies)
        {
            return movies.Select(movie => new FavoriteMovieDto
            {
                MovieId = movie.Id,
                MovieName = movie.Name,
                PosterUrl = movie.PosterImg,
                AddedDate = movie.UserFavorites
                    .OrderByDescending(favorite => favorite.AddedDate)
                    .Select(favorite => favorite.AddedDate)
                    .FirstOrDefault()
            });
        }
    }
}
