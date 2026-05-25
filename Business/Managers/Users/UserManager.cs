using Business.DTOs.Users;
using Business.Mapping;
using Core.Helpers;
using DataAccess.Repositories.User;
using Microsoft.AspNetCore.Identity;

namespace Business.Managers.Users
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<Core.Entities.ApplicationUser> _userManager;

        public UserManager(IUserRepository userRepository, UserManager<Core.Entities.ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<PaginationResult<UserListDto>> GetPagedUsersAsync(int page, int pageSize, string? role = null, string? searchTerm = null)
        {
            var result = await _userRepository.GetPagedUsersAsync(page, pageSize, role, searchTerm);
            
            var userListDtos = new List<UserListDto>();
            
            foreach (var user in result.Items)
            {
                var roles = await _userRepository.GetUserRolesAsync(user.Id);
                string? cinemaName = null;
                
                if (user.CinemaId.HasValue)
                {
                    // Get cinema name if exists - will be handled by separate query for performance
                    cinemaName = $"Cinema {user.CinemaId}"; // Placeholder, will be improved
                }
                
                var userDto = user.ToUserListDto(roles, cinemaName);
                userListDtos.Add(userDto);
            }

            return new PaginationResult<UserListDto>
            {
                Items = userListDtos,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages
            };
        }

        public async Task<UserDetailDto?> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserWithDetailsAsync(id);
            if (user == null) return null;

            var roles = await _userRepository.GetUserRolesAsync(id);
            string? cinemaName = null;
            
            if (user.CinemaId.HasValue)
            {
                cinemaName = user.Cinema?.Name ?? $"Cinema {user.CinemaId}";
            }

            return user.ToUserDetailDto(roles, cinemaName);
        }

        public async Task<UserStatsDto> GetUserStatsAsync()
        {
            var allUsersResult = await _userRepository.GetPagedUsersAsync(1, 1000);
            var users = allUsersResult.Items;
            
            var now = DateTime.Now;
            var thisMonth = users.Count(u => u.CreateAt.Year == now.Year && u.CreateAt.Month == now.Month);
            var thisWeek = users.Count(u => u.CreateAt >= now.AddDays(-7));

            var adminCount = 0;
            var agentCount = 0;
            var customerCount = 0;

            foreach (var user in users)
            {
                var roles = await _userRepository.GetUserRolesAsync(user.Id);
                if (roles.Contains("Admin")) adminCount++;
                else if (roles.Contains("CinemaAgent")) agentCount++;
                else customerCount++;
            }

            return new UserStatsDto
            {
                TotalUsers = users.Count,
                ActiveUsers = users.Count(u => u.IsApproved),
                InactiveUsers = users.Count(u => !u.IsApproved),
                PendingApproval = users.Count(u => !u.IsApproved),
                Admins = adminCount,
                CinemaAgents = agentCount,
                Customers = customerCount,
                NewUsersThisMonth = thisMonth,
                NewUsersThisWeek = thisWeek
            };
        }

        public async Task<bool> UpdateUserRoleAsync(string userId, string role, bool add)
        {
            return add 
                ? await _userRepository.AddToRoleAsync(userId, role)
                : await _userRepository.RemoveFromRoleAsync(userId, role);
        }

        public async Task<bool> ToggleUserApprovalAsync(string userId)
        {
            return await _userRepository.ToggleUserApprovalAsync(userId);
        }

        public async Task<bool> ToggleUserStatusAsync(string userId)
        {
            return await _userRepository.ToggleUserStatusAsync(userId);
        }

        public async Task<List<UserListDto>> SearchUsersAsync(string searchTerm, int maxResults = 50)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm, maxResults);
            return users.Select(u => {
                var roles = _userRepository.GetUserRolesAsync(u.Id).Result;
                return u.ToUserListDto(roles);
            }).ToList();
        }

        public async Task<int> GetUsersCountAsync(string? role = null)
        {
            return await _userRepository.GetUsersCountAsync(role);
        }
    }
}
