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
    }
}
