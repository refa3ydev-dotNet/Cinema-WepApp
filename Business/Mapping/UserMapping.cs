using Business.DTOs.Users;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Mapping
{
    public static class UserMapping
    {
        public static UserListDto ToUserListDto(this ApplicationUser user, IList<string> roles, string? cinemaName = null)
        {
            return new UserListDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                CreateAt = user.CreateAt,
                UpdateAt = user.UpdateAt,
                IsApproved = user.IsApproved,
                IsDeleted = user.IsDeleted,
                CinemaId = user.CinemaId,
                CinemaName = cinemaName,
                Roles = roles,
                TotalBookings = 0, // Will be populated separately
                TotalFavorites = 0  // Will be populated separately
            };
        }

        public static UserDetailDto ToUserDetailDto(this ApplicationUser user, IList<string> roles, string? cinemaName = null)
        {
            return new UserDetailDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                CreateAt = user.CreateAt,
                UpdateAt = user.UpdateAt,
                IsApproved = user.IsApproved,
                IsDeleted = user.IsDeleted,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd.HasValue,
                AccessFailedCount = user.AccessFailedCount,
                CinemaId = user.CinemaId,
                CinemaName = cinemaName,
                Roles = roles,
                SecurityStamp = user.SecurityStamp,
                ConcurrencyStamp = user.ConcurrencyStamp
            };
        }

        public static async Task<UserStatsDto> ToUserStatsDto(
            UserManager<ApplicationUser> userManager,
            IQueryable<ApplicationUser> usersQuery)
        {
            var allUsers = await usersQuery
                .Where(u => !u.IsDeleted)
                .ToListAsync();

            var now = DateTime.Now;
            var thisMonth = allUsers.Count(u => u.CreateAt.Year == now.Year && u.CreateAt.Month == now.Month);
            var thisWeek = allUsers.Count(u => u.CreateAt >= now.AddDays(-7));

            var admins = await userManager.GetUsersInRoleAsync("Admin");
            var agents = await userManager.GetUsersInRoleAsync("CinemaAgent");
            var customers = await userManager.GetUsersInRoleAsync("Customer");

            return new UserStatsDto
            {
                TotalUsers = allUsers.Count,
                ActiveUsers = allUsers.Count(u => u.IsApproved && !u.IsDeleted),
                InactiveUsers = allUsers.Count(u => !u.IsApproved || u.IsDeleted),
                PendingApproval = allUsers.Count(u => !u.IsApproved),
                Admins = admins.Count,
                CinemaAgents = agents.Count,
                Customers = customers.Count,
                NewUsersThisMonth = thisMonth,
                NewUsersThisWeek = thisWeek
            };
        }
    }
}
