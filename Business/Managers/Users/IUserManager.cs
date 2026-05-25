using Business.DTOs.Users;
using Core.Helpers;

namespace Business.Managers.Users
{
    public interface IUserManager
    {
        Task<PaginationResult<UserListDto>> GetPagedUsersAsync(int page, int pageSize, string? role = null, string? searchTerm = null);
        Task<UserDetailDto?> GetUserByIdAsync(string id);
        Task<UserStatsDto> GetUserStatsAsync();
        Task<bool> UpdateUserRoleAsync(string userId, string role, bool add);
        Task<bool> ToggleUserApprovalAsync(string userId);
        Task<bool> ToggleUserStatusAsync(string userId);
        Task<List<UserListDto>> SearchUsersAsync(string searchTerm, int maxResults = 50);
        Task<int> GetUsersCountAsync(string? role = null);
    }
}
