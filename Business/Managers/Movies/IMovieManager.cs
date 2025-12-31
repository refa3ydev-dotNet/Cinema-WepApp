using Business.DTOs.Movies;
using Core.Helpers;

namespace Business.Managers.Movies
{
    public interface IMovieManager
    {
        Task CreateMovieAsync(CreateMovieDto dto);
        Task DeleteMovieAsync(int id);
        Task<List<GetAllMoviesDto>> GetAllMoviesAsync();
        Task<GetMovieByIdDto> GetMovieByIdAsync(int id);
        Task UpdateMovieAsync(UpdateMovieDto dto);
        Task<PaginationResult<GetAllMoviesDto>> GetPagedMoviesAsync(int page, int pageSize);
    }
}
