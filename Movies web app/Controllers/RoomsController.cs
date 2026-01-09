using DataAccess.Contexts;
using DataAccess.Repositories.ROOM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Movies_web_app.Controllers
{
    public class RoomsController : Controller
    {
        private readonly MoviesDbContext _context;
        private readonly IRoomRepository roomRepository;

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
