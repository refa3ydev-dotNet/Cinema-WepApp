using Business.DTOs.Movies;
using Business.Managers.Actors;
using Business.Managers.Categories;
using Business.Managers.Cinemas;
using Business.Managers.Directors;
using Business.Managers.Movies;
using Business.Managers.Producers;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICinemasManager cinemasManager;
        private readonly IMovieManager _movieManager;
        private readonly IProducersManager _producerManager;
        private readonly IDirectorManager _directorManager;
        private readonly IActorsManager _actorManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IImageService _imageService;
        public AdminController(UserManager<ApplicationUser> userManager,
            ICinemasManager cinemasManager,
            ICategoryManager categoryManager,
            IMovieManager movieManager,
            IProducersManager producerManager,
            IDirectorManager directorManager,
            IActorsManager actorManager,
            IImageService imageService)
        {
            this.cinemasManager = cinemasManager;
            _movieManager = movieManager;
            _producerManager = producerManager;
            _directorManager = directorManager;
            _actorManager = actorManager;
            _categoryManager = categoryManager;
            _userManager = userManager;
            _imageService = imageService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PendingRequests()
        {
            var PendingCinemas = await cinemasManager.GetPendingCinemasAsync();
            return View(PendingCinemas);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveCinema(int id)
        {
            await cinemasManager.ApproveCinemaAsync(id);
            return RedirectToAction("PendingRequests");
        }
            [HttpPost]
        public async Task<IActionResult> RejectCinema(int id, string reason)
        {
            if(string.IsNullOrWhiteSpace(reason))
            {
                reason = "Your request has been rejected. Please update your information.";
            }
            await cinemasManager.RejectCinemaAsync(id, reason);
            return RedirectToAction("PendingRequests");
        }
        [HttpGet]
        public async Task<IActionResult> CreateMovies()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Categories = new SelectList(await _categoryManager.GetAllCategoriesAsync(), "Id", "Name");
            ViewBag.Actors = new SelectList(await _actorManager.GetAllActorsAsync(), "Id", "FullName");
            ViewBag.Directors = new SelectList(await _directorManager.GetAllDirectorsAsync(), "Id", "FullName");
            ViewBag.Producers = new SelectList(await _producerManager.GetAllProducersAsync(), "Id", "FullName");


            return View(new CreateMovieDto());
        }
        [HttpPost]
        public async Task<IActionResult> CreateMovies(CreateMovieDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _categoryManager.GetAllCategoriesAsync(), "Id", "Name");
                ViewBag.Actors = new SelectList(await _actorManager.GetAllActorsAsync(), "Id", "FullName");
                ViewBag.Directors = new SelectList(await _directorManager.GetAllDirectorsAsync(), "Id", "FullName");
                ViewBag.Producers = new SelectList(await _producerManager.GetAllProducersAsync(), "Id", "FullName");
                return View(dto);
            }
            if (dto.PosterImg != null)
            {
                dto.PosterUrl = await _imageService.UploadImageAsync(dto.PosterImg, "Movies", ImageType.Poster);
            }
            if (dto.BackgroundImg != null)
            {
                dto.BackgroundUrl = await _imageService.UploadImageAsync(dto.BackgroundImg, "Movies", ImageType.Background);
            }
            await _movieManager.CreateMovieAsync(dto);
            TempData["SuccessMessage"] = "Movie created successfully";
            return RedirectToAction("Dashboard");
        }
    }
}
