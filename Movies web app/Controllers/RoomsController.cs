using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Movies_web_app.Controllers
{
    public class RoomsController : Controller
    {
        private readonly MoviesDbContext _context;

        public RoomsController(MoviesDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var allRooms = await _context.Rooms.ToListAsync();
            return View();
        }
    }
}
