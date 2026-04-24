using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Business.DTOs.Accounts;

public class UserProfileDto
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? ProfilePicture { get; set; }
    public IFormFile? ProfilePictureFile { get; set; }
    
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; } 

    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number")]
    public string? Password { get; set; } 

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The new password and confirmation password do not match")]
    public string? ConfirmPassword { get; set; }

    public DateTime UpdateDate { get; set; }
}