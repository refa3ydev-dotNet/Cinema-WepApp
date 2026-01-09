using Microsoft.AspNetCore.Mvc;

namespace Movies_web_app.Controllers
{
    public class DirectorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
