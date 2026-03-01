using Business.DTOs.Cinemas;
using Business.Mapping;
using Core.Helpers;
using DataAccess.Repositories.CINEMA;

namespace Business.Managers.Cinemas
{
    public class CinemasManager : ICinemasManager
    {
        private readonly ICinemaRepository _cinemaRepository;
        public CinemasManager(ICinemaRepository cinemarepository)
        {
            _cinemaRepository = cinemarepository;
        }

        public async Task<int> CreateCinemaAsync(CreateCinemaDto dto)
        {
            var cinema = dto.ToEntity();
            cinema.ApprovalStatus.Equals("Pending");
            
            await _cinemaRepository.AddCinemaAsync(cinema);
            return cinema.Id;
        }
        public async Task DeleteCinemaAsync(int id)
        {
            var exist = await _cinemaRepository.GetCinemaByIdAsync(id);
            if (exist == null) throw new Exception("Cinema not found");
            await _cinemaRepository.DeleteCinemaAsync(id);
        }
        public async Task<List<GetAllCinemasDto>> GetAllCinemasAsync()
        {
            var cinema = await _cinemaRepository.GetAllCinemasAsync();
            return cinema.ToDto();
        }
        public async Task<GetCinemaByIdDto> GetCinemaByIdAsync(int id)
        {
            if (id<=0) return null;
            var cinema = await _cinemaRepository.GetCinemaByIdAsync(id);
            if (cinema == null) return null;
            return cinema.ToDto();
        }
        public async Task UpdateCinemaAsync(UpdateCinemaDto dto)
        {
            var existing = await _cinemaRepository.GetCinemaByIdAsync(dto.Id);
            if (existing == null) throw new Exception("Cinema not found");

            var cinema = dto.ToEntity();
            await _cinemaRepository.UpdateCinemaAsync(cinema);
        }
        public async Task<PaginationResult<GetAllCinemasDto>> GetPagedCinemasAsync(int page, int pageSize)
        {
            var result = await _cinemaRepository.GetPagedCinemasAsync(page, pageSize);
            var MappedItems = result.Items.ToDto();
            return new PaginationResult<GetAllCinemasDto>
            {
                Items = MappedItems,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages
            };
        }

        public async Task<List<GetAllCinemasDto>> GetPendingCinemasAsync()
        {
            var cinemas =await _cinemaRepository.GetPendingCinemasAsync();
            return cinemas.ToDto();
        }

        public async Task ApproveCinemaAsync(int id)
        {
            await _cinemaRepository.ApproveCinemaAsync(id);
        }
    }
}
