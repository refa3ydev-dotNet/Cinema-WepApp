using Business.DTOs.Users;
using Business.Managers.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movies_web_app.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserManager userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? role = null, string? searchTerm = null)
        {
            try
            {
                var result = await _userManager.GetPagedUsersAsync(page, pageSize, role, searchTerm);
                ViewBag.CurrentRole = role;
                ViewBag.SearchTerm = searchTerm;
                ViewBag.Roles = new[] { "All", "Admin", "CinemaAgent", "Customer" };
                
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users");
                TempData["ErrorMessage"] = "Failed to load users. Please try again.";
                return View(new Core.Helpers.PaginationResult<UserListDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var user = await _userManager.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user details for {UserId}", id);
                TempData["ErrorMessage"] = "Failed to load user details.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleApproval(string userId)
        {
            try
            {
                var result = await _userManager.ToggleUserApprovalAsync(userId);
                if (result)
                {
                    TempData["SuccessMessage"] = "User approval status updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update user approval status.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling approval for user {UserId}", userId);
                TempData["ErrorMessage"] = "An error occurred while updating approval status.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string userId)
        {
            try
            {
                var result = await _userManager.ToggleUserStatusAsync(userId);
                if (result)
                {
                    TempData["SuccessMessage"] = "User status updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update user status.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling status for user {UserId}", userId);
                TempData["ErrorMessage"] = "An error occurred while updating status.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(string userId, string role, bool add)
        {
            try
            {
                var result = await _userManager.UpdateUserRoleAsync(userId, role, add);
                if (result)
                {
                    TempData["SuccessMessage"] = $"User role '{role}' updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to update user role '{role}'.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role for user {UserId}", userId);
                TempData["ErrorMessage"] = "An error occurred while updating role.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var stats = await _userManager.GetUserStatsAsync();
                return Json(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user stats");
                return Json(new UserStatsDto());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm, int maxResults = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return Json(new List<UserListDto>());
                }

                var users = await _userManager.SearchUsersAsync(searchTerm, maxResults);
                return Json(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users with term {SearchTerm}", searchTerm);
                return Json(new List<UserListDto>());
            }
        }
    }
}
