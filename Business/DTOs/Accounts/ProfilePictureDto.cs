using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Accounts
{
    public class ProfilePictureDto
    {
        public string Email { get; set; }=string.Empty;
        
        public string Role { get; set; }=string.Empty;
        public IFormFile? ProfilePictureFile { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
