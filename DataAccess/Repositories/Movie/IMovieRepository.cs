using Core;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
