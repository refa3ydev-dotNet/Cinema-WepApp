using Business.Managers.Cinemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movies_web_app.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICinemasManager cinemasManager;
        public AdminController(ICinemasManager cinemasManager)
        {
            this.cinemasManager = cinemasManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> pendingRequests()
        {
            var PendingCinemas = await cinemasManager.GetPendingCinemasAsync();
            return View(PendingCinemas);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveCinema(int id)
        {
            await cinemasManager.ApproveCinemaAsync(id);
            return RedirectToAction("pendingRequests");
        }
            [HttpPost]
        public async Task<IActionResult> RejectCinema(int id)
        {
            await cinemasManager.DeleteCinemaAsync(id);
            return RedirectToAction("pendingRequests");
        }
    }
}
