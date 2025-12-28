using Microsoft.AspNetCore.Mvc;

namespace Movies_web_app.Controllers
{
    public class DirectorController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
