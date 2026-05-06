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
            ViewData["CurrentFilter"] = "searchString";

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
        public async Task<IActionResult> Create(CreateDirectorDto director)
        {
            if (!ModelState.IsValid)
            {
                return View(director);
            }
            var dirc = new CreateDirectorDto
            {
                Name = director.Name,
                Biography = director.Biography,
                BirthDate = director.BirthDate,
                IMDB = director.IMDB,
                DeathDate = director.DeathDate,
                Nationality = director.Nationality,
            };
            if (director.ProfilePicture != null)
            {
                dirc.ProfilePictureUrl =
                await _imageService.UploadImageAsync(director.ProfilePicture, "Directors", ImageType.Profile);

            }
            if (director.ProfilePicture == null)
            {
                if (director.ProfilePictureUrl != null)
                {
                    dirc.ProfilePictureUrl = director.ProfilePictureUrl;
                }
            }
            await _directorManager.CreateDirectorAsync(dirc);
            return RedirectToAction("Index");
        }
    }
}
