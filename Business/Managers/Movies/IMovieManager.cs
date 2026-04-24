using Business.DTOs.Movies;
using Core;
using Core.Helpers;

namespace Business.Managers.Movies
{
    public interface 
        IMovieManager
    {
        Task CreateMovieAsync(CreateMovieDto dto);
        Task DeleteMovieAsync(int id);
        Task<List<GetAllMoviesDto>> GetAllMoviesAsync();
        Task<GetMovieByIdDto> GetMovieByIdAsync(int id);
        Task UpdateMovieAsync(UpdateMovieDto dto);
        Task<PaginationResult<GetAllMoviesDto>> GetPagedMoviesAsync(int page, int pageSize);
        Task AssignMovieToCinemaAsync(int movieId, int cinemaId);
        Task<List<GetAllMoviesDto>> GetMoviesByCinemaIdAsync(int cinemaId);
        Task<bool> RemoveMovieFromCinemaAsync(int movieId, int cinemaId); // عشان زرار الحذف
        Task SyncMovieFromTmdbAsync(int tmdbMovieId, int cinemaId);
        Task<IEnumerable<CustomerMovieCatalogDto>> GetAllCatalogMoviesAsync();

    }
}
