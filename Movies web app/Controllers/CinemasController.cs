using Business.DTOs.Cinemas;
using Business.Managers.Cinemas;
using Core.Enums;
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
        [HttpGet]
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
            var cinemaModel = new CreateCinemaDto
            {
                Name = cinema.Name,
                Description = cinema.Description,
                Address = cinema.Address
            };
            if(cinema.Logo != null)
            {
                cinemaModel.LogoPath =
await _imageServises.UploadImageAsync(cinema.Logo, "Cinemas",ImageType.Profile);

            }
            if (cinema.BackgroundPicture != null)
            {
                cinemaModel.BackgroundPath =
await _imageServises.UploadImageAsync(cinema.BackgroundPicture, "Cinemas",ImageType.Background);

            }
            if (cinema.Logo==null)
            {
                cinemaModel.LogoPath = cinema.LogoPath;
            }
            if (cinema.BackgroundPicture == null)
            {
                cinemaModel.BackgroundPath = cinema.BackgroundPath;
            }
            await _cinemaManager.CreateCinemaAsync(cinemaModel);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            if(id<=0) return View("NotFound");
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
        public async Task<IActionResult> Edit(UpdateCinemaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var exsistingCinema = await _cinemaManager.GetCinemaByIdAsync(dto.Id);
            if (exsistingCinema == null) return View("NotFound");
            if (dto.Logo != null)
            {
                if (!string.IsNullOrEmpty(dto.LogoPath) && !exsistingCinema.LogoPath.StartsWith("http"))
                {
                    await _imageServises.DeleteImageAsync(exsistingCinema.LogoPath);
                }

                dto.LogoPath =
                await _imageServises.UploadImageAsync(dto.Logo, "Cinemas", ImageType.Profile);


            }
            else if(!string.IsNullOrEmpty(dto.LogoPath ) && dto.LogoPath != exsistingCinema.LogoPath)
            {
                if(!string.IsNullOrEmpty(exsistingCinema.LogoPath) && !exsistingCinema.LogoPath.StartsWith("http"))
                {
                    await _imageServises.DeleteImageAsync(exsistingCinema.LogoPath);
                }
            }

            if(dto.BackgroundPicture != null)
            {
                if (!string.IsNullOrEmpty(exsistingCinema.BackgroundPath) && !exsistingCinema.BackgroundPath.StartsWith("http"))
                {
                    await _imageServises.DeleteImageAsync(exsistingCinema.BackgroundPath);
                }

                    dto.BackgroundPath =
                    await _imageServises.UploadImageAsync(dto.BackgroundPicture, "Cinemas", ImageType.Background);

            }
            else if(!string.IsNullOrEmpty(dto.BackgroundPath) && dto.BackgroundPath != exsistingCinema.BackgroundPath)
            {
                if (!string.IsNullOrEmpty(exsistingCinema.BackgroundPath) && !exsistingCinema.BackgroundPath.StartsWith("http"))
                {
                    await _imageServises.DeleteImageAsync(exsistingCinema.BackgroundPath);
                }
            }


            await _cinemaManager.UpdateCinemaAsync(dto);
            return RedirectToAction("Details", new { id = dto.Id });

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _cinemaManager.GetCinemaByIdAsync(id);
            if (cinema == null) return View("NotFound");
            if (!string.IsNullOrEmpty(cinema.LogoPath))
            {
                await _imageServises.DeleteImageAsync(cinema.LogoPath);
            }
            if (!string.IsNullOrEmpty(cinema.BackgroundPath))
            {
                await _imageServises.DeleteImageAsync(cinema.BackgroundPath);
            }
            await _cinemaManager.DeleteCinemaAsync(id);
            return RedirectToAction("Index");
        }
    }
}
