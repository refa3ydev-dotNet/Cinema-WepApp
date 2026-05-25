using Core.Entities;
using Core.Helpers;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly MoviesDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(MoviesDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<PaginationResult<ApplicationUser>> GetPagedUsersAsync(int page, int pageSize, string? role = null, string? searchTerm = null)
    {
        var query = _context.Users
            .Where(u => !u.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            var roleIds = usersInRole.Select(u => u.Id).ToHashSet();
            query = query.Where(u => roleIds.Contains(u.Id));
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(u => 
                (u.FirstName != null && u.FirstName.Contains(searchTerm)) ||
                (u.LastName != null && u.LastName.Contains(searchTerm)) ||
                (u.Email != null && u.Email.Contains(searchTerm)) ||
                (u.UserName != null && u.UserName.Contains(searchTerm))
            );
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var users = await query
            .OrderByDescending(u => u.CreateAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginationResult<ApplicationUser>
        {
            Items = users,
            CurrentPage = page,
            TotalPages = totalPages
        };
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<ApplicationUser?> GetUserWithDetailsAsync(string id)
    {
        return await _context.Users
            .Include(u => u.Cinema)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<int> GetUsersCountAsync(string? role = null)
    {
        var query = _context.Users
            .Where(u => !u.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            var roleIds = usersInRole.Select(u => u.Id).ToHashSet();
            query = query.Where(u => roleIds.Contains(u.Id));
        }

        return await query.CountAsync();
    }

    public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string role)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
        return usersInRole
            .Where(u => !u.IsDeleted)
            .ToList();
    }

    public async Task<bool> IsUserInRoleAsync(string userId, string role)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;
        
        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return new List<string>();
        
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> AddToRoleAsync(string userId, string role)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;
        
        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> RemoveFromRoleAsync(string userId, string role)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;
        
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> ToggleUserApprovalAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;
        
        user.IsApproved = !user.IsApproved;
        user.UpdateAt = DateTime.Now;
        
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    public async Task<bool> ToggleUserStatusAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;
        
        user.IsDeleted = !user.IsDeleted;
        user.DeleteAt = user.IsDeleted ? DateTime.Now : null;
        user.UpdateAt = DateTime.Now;
        
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    public async Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm, int maxResults = 50)
    {
        return await _context.Users
            .Where(u => !u.IsDeleted &&
                ((u.FirstName != null && u.FirstName.Contains(searchTerm)) ||
                (u.LastName != null && u.LastName.Contains(searchTerm)) ||
                (u.Email != null && u.Email.Contains(searchTerm)) ||
                (u.UserName != null && u.UserName.Contains(searchTerm))))
            .Take(maxResults)
            .AsNoTracking()
            .ToListAsync();
    }
}
