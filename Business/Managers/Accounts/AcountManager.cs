using Business.DTOs.Accounts;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Managers.Accounts
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateProfileAsync(string userId, UserProfileDto dto)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user==null)return (false, "User not found");
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            if (!string.IsNullOrEmpty(dto.ProfilePicture))
            {
                user.ProfilePictureUrl = dto.ProfilePicture;
            }

            if (!string.IsNullOrEmpty(dto.CurrentPassword) && !string.IsNullOrEmpty(dto.Password))
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.Password);

                if (!passwordResult.Succeeded)
                {
                    return (false, passwordResult.Errors.FirstOrDefault()?.Description);
                }
            }
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return (false, updateResult.Errors.FirstOrDefault()?.Description);
            }
            return (true, "Profile successfully updated");
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto)
        {
            bool IsAutoApproved = registerDto.Role == "Customer";
            var newUser = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                IsApproved = IsAutoApproved,
                Gender=registerDto.Gender,
                UserName = registerDto.Email.Split('@')[0]+new Random().Next(1000, 9999).ToString(),
            };
            var result =await _userManager.CreateAsync(newUser, registerDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, registerDto.Role);
            }
            return result;
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
