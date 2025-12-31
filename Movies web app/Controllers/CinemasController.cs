using Business.DTOs.Cinemas;
using Business.Managers.Cinemas;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    public class CinemasController : Controller
    {
        private readonly MoviesDbContext _context;
        private readonly ICinemasManager _cinemaManager;
        private readonly IImageService _imageServises;



        public CinemasController(MoviesDbContext context, ICinemasManager cinemaManager, IImageService imageServices)
        {
            _context = context;
            _cinemaManager = cinemaManager;
            _imageServises = imageServices;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var allCinemas = await _cinemaManager.GetPagedCinemasAsync(page, pageSize);
            return View(allCinemas);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCinemaDto cinema)
        {
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            string logoImageName = await _imageServises.UploadImageAsync(cinema.Logo, "Cinemas");
            string backgroundImageName = await _imageServises.UploadImageAsync(cinema.BackgroundPicture, "Cinemas");
            var cinemaModel = new CreateCinemaDto
            {
                LogoPath = "/Images/Cinemas/" + logoImageName,
                Name = cinema.Name,
                Description = cinema.Description,
                BackgroundPath = "/Images/Cinemas/" + backgroundImageName,
                Address = cinema.Address
            };
            await _cinemaManager.CreateCinemaAsync(cinemaModel);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            var cinema = await _cinemaManager.GetCinemaByIdAsync(id);
            if (cinema == null) return View("NotFound");
            return View(cinema);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cinema = await _cinemaManager.GetCinemaByIdAsync(id);
            if (cinema == null) return View("NotFound");
            var dto = new UpdateCinemaDto
            {
                Id = cinema.Id,
                LogoPath = cinema.LogoPath,
                Name = cinema.Name,
                Description = cinema.Description,
                BackgroundPath = cinema.BackgroundPath,
                Address = cinema.Address
            };
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCinemaDto cinema)
        {
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            if (cinema.Logo != null)
            {
                if (!string.IsNullOrEmpty(cinema.LogoPath))
                {
                    await _imageServises.DeleteImageAsync(cinema.LogoPath);
                }
                string logoImageName = await _imageServises.UploadImageAsync(cinema.Logo, "Cinemas");
                cinema.LogoPath = "/Images/Cinemas/" + logoImageName;

            }
            if (cinema.BackgroundPicture != null)
            {
                if (!string.IsNullOrEmpty(cinema.BackgroundPath))
                {
                    await _imageServises.DeleteImageAsync(cinema.BackgroundPath);
                }
                string backgroundImageName = await _imageServises.UploadImageAsync(cinema.BackgroundPicture, "Cinemas");
                cinema.BackgroundPath = "/Images/Cinemas/" + backgroundImageName;

            }

            await _cinemaManager.UpdateCinemaAsync(cinema);
            return RedirectToAction("Details", new { id = cinema.Id });

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _cinemaManager.GetCinemaByIdAsync(id);
            if (cinema == null) return View("NotFound");
            if (!string.IsNullOrEmpty(cinema.LogoPath))
            {
                var relativePath = Path.Combine("wwwroot", cinema.LogoPath);
                Console.WriteLine("Path: " + cinema.LogoPath);
                await _imageServises.DeleteImageAsync(relativePath);
            }
            if (!string.IsNullOrEmpty(cinema.BackgroundPath))
            {
                var relativePath = Path.Combine("wwwroot", cinema.BackgroundPath);
                Console.WriteLine("Path: " + cinema.BackgroundPath);
                await _imageServises.DeleteImageAsync(relativePath);
            }
            await _cinemaManager.DeleteCinemaAsync(id);
            return RedirectToAction("Index");
        }
    }
}
