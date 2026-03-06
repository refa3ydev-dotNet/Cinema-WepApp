using Business.Managers.Agent;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Movies_web_app.Controllers
{
        [Authorize(Roles = "CinemaAgent")]
    public class AgentController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAgentDashboardManager _dashboardManager;
        
        public AgentController(UserManager<ApplicationUser> userManager, IAgentDashboardManager dashboardManager)
        {
            _userManager = userManager;
            _dashboardManager = dashboardManager;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("Login","Account");
            }

            var dashboardDto = await _dashboardManager.GetAgentDashboardDataAsync(user.CinemaId.Value,user.FirstName);

            return View(dashboardDto);
        }

    }
}
