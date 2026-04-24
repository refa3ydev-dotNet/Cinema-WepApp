using Business.DTOs.Accounts;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Managers.Accounts
{
    public interface IAccountManager
    {
         Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto);
         Task<ApplicationUser> GetUserByEmailAsync(string email);
         Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
         Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<(bool IsSuccess,String ErrorMessage)> UpdateProfileAsync(string userId, UserProfileDto dto);

    }
}
