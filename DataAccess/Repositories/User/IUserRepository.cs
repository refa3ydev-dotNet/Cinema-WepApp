using Core.Entities;
using Core.Helpers;

namespace DataAccess.Repositories.User;

public interface IUserRepository
{
    Task<PaginationResult<ApplicationUser>> GetPagedUsersAsync(int page, int pageSize, string? role = null, string? searchTerm = null);
    Task<ApplicationUser?> GetUserByIdAsync(string id);
    Task<ApplicationUser?> GetUserWithDetailsAsync(string id);
    Task<int> GetUsersCountAsync(string? role = null);
    Task<List<ApplicationUser>> GetUsersByRoleAsync(string role);
    Task<bool> IsUserInRoleAsync(string userId, string role);
    Task<IList<string>> GetUserRolesAsync(string userId);
    Task<bool> AddToRoleAsync(string userId, string role);
    Task<bool> RemoveFromRoleAsync(string userId, string role);
    Task<bool> ToggleUserApprovalAsync(string userId);
    Task<bool> ToggleUserStatusAsync(string userId);
    Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm, int maxResults = 50);
}
