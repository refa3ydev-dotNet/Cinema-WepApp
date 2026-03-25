using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.DTOs.Accounts
{
    public  class LoginDto
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]

        public string Email { get; set; }=string.Empty;
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }=string.Empty;
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
