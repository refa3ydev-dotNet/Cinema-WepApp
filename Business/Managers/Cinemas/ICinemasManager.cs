using Business.DTOs.Cinemas;
using Core.Helpers;

namespace Business.Managers.Cinemas
{
    public interface ICinemasManager
    {
        Task CreateCinemaAsync(CreateCinemaDto dto);
        Task DeleteCinemaAsync(int id);
        Task<List<GetAllCinemasDto>> GetAllCinemasAsync();
        Task<GetCinemaByIdDto> GetCinemaByIdAsync(int id);
        Task UpdateCinemaAsync(UpdateCinemaDto dto);
        Task<PaginationResult<GetAllCinemasDto>> GetPagedCinemasAsync(int page, int pageSize);
    }
}
