using Business.DTOs.Accounts;
using Business.Managers.Accounts;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;
using System.Security.Claims;

namespace Movies_web_app.Controllers
{
    public class AccountController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IAccountManager _accountManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountManager accountManager, SignInManager<ApplicationUser> signInManager, IImageService imageService)
        {
            _accountManager = accountManager;
            _signInManager = signInManager;
            _imageService = imageService;
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
            var custumClaims = new List<Claim>
                        {
                            new Claim("FirstName",user.FirstName??""),
                            new Claim("LastName",user.LastName??""),
                            new Claim("ProfilePictureURL",user.ProfilePictureUrl??(user.Gender=="Male"?"/images/male.png":"/images/female.png"))
                        };
            await _signInManager.SignInWithClaimsAsync(user, isPersistent: true, custumClaims);
            
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
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var user = await _accountManager.GetUserByEmailAsync(dto.Email);

            if(user != null)
            {

                var passwordCheck=await _accountManager.CheckPasswordAsync(user,dto.Password);
                if (passwordCheck)
                {
                    if(Request.Cookies.TryGetValue("AutoRememberMe",out string cookieValue))
                    {
                        bool isPersistent = cookieValue == "true";
                        var custumClaims = new List<Claim>
                        {
                            new Claim("FirstName",user.FirstName??""),
                            new Claim("LastName",user.LastName??""),
                            new Claim("ProfilePictureURL",user.ProfilePictureUrl??(user.Gender=="Male"?"/images/male.png":"/images/female.png"))
                        };
                        await _signInManager.SignInWithClaimsAsync(user, isPersistent: isPersistent, custumClaims);
                        
                        var userRoles= await _accountManager.GetUserRolesAsync(user);
                        if(userRoles.Contains("CinemaAgent")&& user.CinemaId == null)
                        {
                            return RedirectToAction("Create", "Cinemas");
                        }
                        return RedirectToAction("Index", "Home");

                    }
                    return RedirectToAction("RememberMe", new { email = dto.Email });
                    //var result =await _signInManager.PasswordSignInAsync(user,dto.Password,isPersistent:dto.RememberMe,lockoutOnFailure:false);
                    //if (result.Succeeded)
                    //{
                    //    return RedirectToAction("Index", "Home");
                    //}
                }
            }
            ModelState.AddModelError("","Invalid Email or Password");
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> RememberMe(string email)
        {

            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Account");
            ViewBag.Email = email;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RememberMe(string email, string actionType ,string DontShowAgain)
        {
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Account");
            var user = await _accountManager.GetUserByEmailAsync(email);
            if (user == null) return RedirectToAction("Login", "Account");
            bool isPersistent = actionType == "Yes";
            if (DontShowAgain == "on")
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                    
                };
                Response.Cookies.Append("AutoRememberMe", isPersistent.ToString(), options);
            }
            var custumClaims = new List<Claim>
                        {
                            new Claim("FirstName",user.FirstName??""),
                            new Claim("LastName",user.LastName??""),
                            new Claim("ProfilePictureURL",user.ProfilePictureUrl??(user.Gender=="Male"?"/images/male.png":"/images/female.png"))
                        };
            await _signInManager.SignInWithClaimsAsync(user, isPersistent: isPersistent, custumClaims);
            var userRoles = await _accountManager.GetUserRolesAsync(user);
            if (userRoles.Contains("CinemaAgent") && user.CinemaId == null)
            {
                return RedirectToAction("Create", "Cinemas");
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
