using Business.DTOs.Accounts;
using Business.Managers.Accounts;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Movies_web_app.Controllers
{
    public class AccountController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IAccountManager _accountManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataProtector _pendingLoginProtector;

        public AccountController(IAccountManager accountManager, SignInManager<ApplicationUser> signInManager,
            IImageService imageService, IDataProtectionProvider dataProtectionProvider)
        {
            _accountManager = accountManager;
            _signInManager = signInManager;
            _imageService = imageService;
            _pendingLoginProtector = dataProtectionProvider.CreateProtector("Account.PendingLogin");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View(dto);

            var userExists = await _accountManager.GetUserByEmailAsync(dto.Email);
            if (userExists != null)
            {
                ModelState.AddModelError("Email", "Email already in use");
                return View(dto);
            }

            var result = await _accountManager.RegisterUserAsync(dto);
            if (result.Succeeded)
            {
                var newUser = await _accountManager.GetUserByEmailAsync(dto.Email);
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("SetupProfile", new { email = dto.Email, Role = dto.Role });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(dto);
            }
        }

        [HttpGet]
        public IActionResult SetupProfile(string email, string Role)
        {
            var model = new ProfilePictureDto
            {
                Email = email,
                Role = Role
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetupProfile(ProfilePictureDto dto, string actionType)
        {
            var user = await _accountManager.GetUserByEmailAsync(dto.Email);
            if (user == null) return View("NotFound");

            if (actionType == "Save")
            {
                if (dto.ProfilePictureFile != null)
                {
                    user.ProfilePictureUrl = await _imageService.UploadImageAsync(dto.ProfilePictureFile, "Users", ImageType.Profile);
                }
                else if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
                {
                    user.ProfilePictureUrl = dto.ProfilePictureUrl;
                }
                await _accountManager.UpdateUserAsync(user);
            }

            await _signInManager.SignInAsync(user, isPersistent: true);

            if (dto.Role == "CinemaAgent")
            {
                return RedirectToAction("Create", "Cinemas");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginDto dto)
{
    if (!ModelState.IsValid) return View(dto);

    var user = await _accountManager.GetUserByEmailAsync(dto.Email);
    if (user == null)
    {
        ModelState.AddModelError("", "Invalid email or password. Please try again.");
        return View(dto);
    }

    if (!user.IsApproved)
    {
        ModelState.AddModelError("", "Your account is pending admin approval. Please wait for activation.");
        return View(dto);
    }
    
    if (user.IsDeleted)
    {
        ModelState.AddModelError("", "This account has been deactivated. Please contact support.");
        return View(dto);
    }

    var signInCheck = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

    if (!signInCheck.Succeeded)
    {
        if (signInCheck.IsLockedOut)
        {
            ModelState.AddModelError("", "Account locked due to multiple failed attempts. Please try again later.");
        }
        else if (signInCheck.RequiresTwoFactor)
        {
            ModelState.AddModelError("", "Two-factor authentication required. Please use your authenticator app.");
        }
        else if (signInCheck.IsNotAllowed)
        {
            ModelState.AddModelError("", "Login not allowed for this account. Please contact support.");
        }
        else
        {
            ModelState.AddModelError("", "Invalid email or password. Please check your credentials and try again.");
        }
        return View(dto);
    }

    if (Request.Cookies.TryGetValue("AutoRememberMe", out string cookieValue))
    {
        bool isPersistent = cookieValue == "true";
        await _signInManager.SignInAsync(user, isPersistent: isPersistent);
        return await RedirectAfterLogin(user);
    }

    var pendingLoginData = new PendingLoginData
    {
        UserId = user.Id,
        SecurityStamp = user.SecurityStamp,
        Expiration = DateTimeOffset.UtcNow.AddMinutes(5)
    };
    var json = JsonSerializer.Serialize(pendingLoginData);
    var protectedToken = _pendingLoginProtector.Protect(json);

    TempData["PendingLoginToken"] = protectedToken;
    Response.Cookies.Append("PendingLoginToken", protectedToken, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddMinutes(10)
    });
    return RedirectToAction("RememberMe");
}

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RememberMe()
        {
            // Read token from cookie
            if (!Request.Cookies.TryGetValue("PendingLoginToken", out var protectedToken))
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(protectedToken))
            {
                return RedirectToAction("Login", "Account");
            }

            // Validate token
            try
            {
                var json = _pendingLoginProtector.Unprotect(protectedToken);
                var pendingLoginData = JsonSerializer.Deserialize<PendingLoginData>(json);
                if (pendingLoginData == null || pendingLoginData.Expiration < DateTimeOffset.UtcNow)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }

            // Pass token to view via ViewBag
            ViewBag.PendingLoginToken = protectedToken;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RememberMe(string actionType, string DontShowAgain)
        {
            if (string.IsNullOrEmpty(actionType))
            {
                return RedirectToAction("Login", "Account");
            }

            // Read token from cookie
            if (!Request.Cookies.TryGetValue("PendingLoginToken", out var protectedToken))
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(protectedToken))
            {
                return RedirectToAction("Login", "Account");
            }

            PendingLoginData pendingLoginData;
            try
            {
                var json = _pendingLoginProtector.Unprotect(protectedToken);
                pendingLoginData = JsonSerializer.Deserialize<PendingLoginData>(json);
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }

            if (pendingLoginData == null || pendingLoginData.Expiration < DateTimeOffset.UtcNow)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _accountManager.GetUserByIdAsync(pendingLoginData.UserId);
            if (user == null || user.SecurityStamp != pendingLoginData.SecurityStamp || !user.IsApproved)
            {
                return RedirectToAction("Login", "Account");
            }

            bool isPersistent = actionType == "Yes";
            if (DontShowAgain == "on")
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };
                Response.Cookies.Append("AutoRememberMe", isPersistent.ToString(), options);
            }

            // Remove the pending login cookie
            Response.Cookies.Delete("PendingLoginToken");

            await _signInManager.SignInAsync(user, isPersistent: isPersistent);
            return await RedirectAfterLogin(user);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login");
            var user = await _accountManager.GetUserByIdAsync(userId);
            if (user == null) return RedirectToAction("Login");

            var dto = new UserProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicture = user.ProfilePictureUrl
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login");
            if (!ModelState.IsValid) return View(dto);

            var user = await _accountManager.GetUserByIdAsync(userId);
            if (user == null) return RedirectToAction("Login");

            if (dto.ProfilePictureFile != null && dto.ProfilePictureFile.Length > 0)
            {
                if (dto.ProfilePictureFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProfilePictureFile", "Image size cannot exceed 2MB.");
                    return View(dto);
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(dto.ProfilePictureFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ProfilePictureFile", "Only JPG and PNG images are allowed.");
                    return View(dto);
                }

                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    await _imageService.DeleteImageAsync(user.ProfilePictureUrl);
                }

                dto.ProfilePicture = await _imageService.UploadImageAsync(
                    dto.ProfilePictureFile,
                    "Users",
                    ImageType.Profile);
            }
            else
            {
                dto.ProfilePicture = user.ProfilePictureUrl;
            }

            var result = await _accountManager.UpdateProfileAsync(userId, dto);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return View(dto);
            }

            if (!string.IsNullOrEmpty(dto.Password))
            {
                await _signInManager.RefreshSignInAsync(user);
            }
            TempData["SuccessMessage"] = "Profile update Successfully!";
            return RedirectToAction("Profile");
        }

        private async Task<IActionResult> RedirectAfterLogin(ApplicationUser user)
        {
            var userRoles = await _accountManager.GetUserRolesAsync(user);
            if (userRoles.Contains("CinemaAgent") && user.CinemaId == null)
            {
                return RedirectToAction("Create", "Cinemas");
            }
            return RedirectToAction("Index", "Home");
        }

        private class PendingLoginData
        {
            public string UserId { get; set; }
            public string SecurityStamp { get; set; }
            public DateTimeOffset Expiration { get; set; }
        }
    }
}
