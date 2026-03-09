using Business.DTOs.Cinemas;
using Business.Mapping;
using Core.Entities;
using Core.Enums;
using Core.Helpers;
using DataAccess.Repositories.CINEMA;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Managers.Cinemas
{
    public class CinemasManager : ICinemasManager
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CinemasManager(ICinemaRepository cinemarepository, UserManager<ApplicationUser> userManager)
        {
            _cinemaRepository = cinemarepository;
            _userManager = userManager;
        }

        public async Task<int> CreateCinemaAsync(CreateCinemaDto dto)
        {
            var cinema = dto.ToEntity();
            cinema.ApprovalStatus=ApprovalStatus.Pending;
            
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
        public async Task<GetCinemaByIdDto> GetApprovedCinemaByIdAsync(int id)
        {
            var cinema = await _cinemaRepository.GetApprovedCinemabyIdAsync(id);
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
        public async Task UpdateCinemaAsync(FixCinemaApplicationDto dto)
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
            if(!cinemas.Any()) return new List<GetAllCinemasDto>();
            var cinemaIds=cinemas.Select(x => x.Id).ToList();
            var agentDictionary=await _userManager.Users
                .Where(u=>u.CinemaId.HasValue&&cinemaIds.Contains(u.CinemaId.Value))
                .ToDictionaryAsync(
                    u => u.CinemaId.Value,
                    u => $"{u.FirstName} {u.LastName}"
                );

            var dtos = cinemas.ToDto();
            foreach (var dto in dtos)
            {
                dto.AgentName = agentDictionary.ContainsKey(dto.Id) ? agentDictionary[dto.Id] : "Unknown Agent";

            }
            return dtos;
        }
        public async Task RejectCinemaAsync(int id ,string reason)
        {
        var cinema = await _cinemaRepository.GetCinemaByIdAsync(id);
            if (cinema==null) return;
            else
            {
                cinema.ApprovalStatus=ApprovalStatus.Rejected;
                cinema.RejectionReason=reason;
                 
                await _cinemaRepository.UpdateCinemaAsync(cinema);
            }
        }
        public async Task ApproveCinemaAsync(int id)
        {
            var cinema =await _cinemaRepository.GetCinemaByIdAsync(id);
            if (cinema!=null)
            {
                cinema.ApprovalStatus = ApprovalStatus.Approved;
                
            }
            await _cinemaRepository.ApproveCinemaAsync(id);
        }
    }
}
