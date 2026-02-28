using Core;
using Core.Helpers;


namespace DataAccess.Repositories.CINEMA
{
    public interface ICinemaRepository
    {
        Task<Cinema> GetCinemaByIdAsync(int id);
        Task<List<Cinema>> GetAllCinemasAsync();
        Task AddCinemaAsync(Cinema cinema);
        Task UpdateCinemaAsync(Cinema cinema);
        Task DeleteCinemaAsync(int id);
        Task<List<Cinema>> SearchByNameAsync(string name);
        Task<List<Cinema>> SearchByMovieAsync(string name);
        Task<int> GetCinemasCountAsync();
        Task <List<Cinema>> GetPendingCinemasAsync();
        Task ApproveCinemaAsync(int id);
        Task DeActivateCinemaAsync(int id);
        Task<PaginationResult<Cinema>> GetPagedCinemasAsync(int page, int pageSize);
    }
}
