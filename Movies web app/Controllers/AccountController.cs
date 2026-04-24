using Business.DTOs.Accounts;
using Business.Managers.Accounts;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
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
            if (!ModelState.IsValid)return View(dto);
            var userExists= await _accountManager.GetUserByEmailAsync(dto.Email);
            if (userExists != null)
            {
                ModelState.AddModelError("Email", "Email already in use");
                return View(dto);
            }
            var result = await _accountManager.RegisterUserAsync(dto);
            if (result.Succeeded)
            {
                var newUser= await _accountManager.GetUserByEmailAsync(dto.Email);
                await _signInManager.SignInAsync(newUser, isPersistent :false);
                return RedirectToAction("SetupProfile", new { email = dto.Email ,Role=dto.Role });
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
        public IActionResult SetupProfile(string email,string Role)
        {
            var model = new ProfilePictureDto
            {
                Email = email,
                Role = Role
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SetupProfile(ProfilePictureDto dto,string actionType)
        {
            var user = await _accountManager.GetUserByEmailAsync(dto.Email);
            if(user ==null) return View("NotFound");

            if (actionType == "Save")
            {
                if (dto.ProfilePictureFile != null)
                {
                    user.ProfilePictureUrl=await _imageService.UploadImageAsync(dto.ProfilePictureFile, "Users", ImageType.Profile);
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
            if(User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
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
                ModelState.AddModelError("","Invalid Email or Password");
                return View(dto);
            }

            if (!user.IsApproved)
            {
                ModelState.AddModelError("","Your account is pending approval.");
                return View(dto);
            }

            var signInCheck = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password
                , lockoutOnFailure: true);

            if (!signInCheck.Succeeded)
            {
                ModelState.AddModelError("","Invalid Email or Password");
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

            if (TempData.Peek("PendingLoginToken") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RememberMe(string actionType ,string DontShowAgain)
        {

            var protectedToken = TempData["PendingLoginToken"] as string;
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
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (user.SecurityStamp != pendingLoginData.SecurityStamp)
            {
                return RedirectToAction("Login", "Account");
                
            }

            if (!user.IsApproved)
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
                Response.Cookies.Append("AutoRememberMe",isPersistent.ToString(),options);
                
            }
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
            if(!ModelState.IsValid) return View(dto);
            var user = await _accountManager.GetUserByIdAsync(userId);
            if (user == null) return RedirectToAction("Login");

            if (dto.ProfilePictureFile != null && dto.ProfilePictureFile.Length > 0)
            {
                if (dto.ProfilePictureFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProfilePictureFile","Image size cannot exceed 2MB.");
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
            if (userRoles.Contains(("CinemaAgent")) && user.CinemaId == null)
            {
                return RedirectToAction("Create", "Cinemas");
                
            }
            return RedirectToAction("Index", "Home");
        }
        private class  PendingLoginData
        {
            public string UserId { get; set; }
            public string SecurityStamp { get; set; }
            public DateTimeOffset Expiration { get; set; }
        }
    }
}
