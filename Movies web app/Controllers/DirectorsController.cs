using Business.DTOs.Directors;
using Business.Managers.Directors;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly IDirectorManager _directorManager;
        private readonly IImageService _imageService;

        public DirectorsController(IDirectorManager directorManager, IImageService imageService)
        {
            _directorManager = directorManager;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            List<GetAllDirectorDto> directors;

            if (!string.IsNullOrEmpty(searchString))
            {
                directors = await _directorManager.SearchDirectorByNameAsync(searchString);
            }
            else
            {
                directors = await _directorManager.GetAllDirectorsAsync();
            }
            return View(directors);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var director = await _directorManager.GetDirectorByIdAsync(id);
            if (director == null) return View("NotFound");
            return View(director);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDirectorDto director)
        {
            if (!ModelState.IsValid)
            {
                return View(director);
            }

            if (director.ProfilePicture != null)
            {
                director.ProfilePictureUrl = await _imageService.UploadImageAsync(director.ProfilePicture, "Directors", ImageType.Profile);
            }
            else if (director.ProfilePictureUrl != null)
            {
                director.ProfilePictureUrl = director.ProfilePictureUrl;
            }

            await _directorManager.CreateDirectorAsync(director);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var director = await _directorManager.GetDirectorByIdAsync(id);
            if (director == null) return View("NotFound");

            if (!string.IsNullOrEmpty(director.ProfilePictureUrl))
            {
                await _imageService.DeleteImageAsync(director.ProfilePictureUrl);
            }

            await _directorManager.DeleteDirectorAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var director = await _directorManager.GetDirectorByIdAsync(id);
            if (director == null) return View("NotFound");

            var dto = new UpdateDirectorDto
            {
                Id = director.Id,
                Name = director.Name,
                Biography = director.Biography,
                ProfilePictureUrl = director.ProfilePictureUrl,
                IMDB = director.IMDB,
                DeathDate = director.DeathDate,
                Nationality = director.Nationality
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateDirectorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if (dto.ProfilePicture != null)
            {
                if (!string.IsNullOrEmpty(dto.ProfilePictureUrl))
                {
                    await _imageService.DeleteImageAsync(dto.ProfilePictureUrl);
                }
                dto.ProfilePictureUrl = await _imageService.UploadImageAsync(dto.ProfilePicture, "Directors", ImageType.Profile);
            }

            await _directorManager.UpdateDirectorAsync(dto);
            return RedirectToAction("Details", new { id = dto.Id });
        }
    }
}
