using Core;
using Core.Helpers;

namespace DataAccess.Repositories.MOVIE
{
    public interface IMovieRepository
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<List<Movie>> GetAllMoviesAsync();
        Task AddMovieAsync(Movie producer);
        Task UpdateMovieAsync(Movie producer);
        Task DeleteMovieAsync(int id);
        Task<List<Movie>> SearchByNameAsync(string name);
        Task<int> GetMoviesCountAsync();
        Task<PaginationResult<Movie>> GetPagedMoviesAsync(int page, int pageSize);
        Task<List<Movie>> GetMovieByActorAsync(string name);
        Task<bool> HasActiveSchedulesForCinemaAsync(int movieId, int cinemaId);
        Task<Movie> UpsertMovieFromTmdbAsync(Movie movie);
    }
}
