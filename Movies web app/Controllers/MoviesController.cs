using Business.DTOs.Movies;
using Business.Managers.Actors;
using Business.Managers.Categories;
using Business.Managers.Cinemas;
using Business.Managers.Movies;
using Business.Managers.Producers;
using Core.Entities.Helpers;
using Core.Enums;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesDbContext _context;
        private readonly IMovieManager _movieManager;
        private readonly IActorsManager _actorManager;
        private readonly ICinemasManager _cinemaManager;
        private readonly IProducersManager _producerManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IImageService _imageServises;
        public MoviesController(MoviesDbContext context, IMovieManager movieManager, IImageService imageService, IActorsManager actorManager, ICinemasManager cinemaManager, IProducersManager producerManager, ICategoryManager categoryManager)
        {
            _context = context;
            _movieManager = movieManager;
            _imageServises = imageService;
            _actorManager = actorManager;
            _cinemaManager = cinemaManager;
            _producerManager = producerManager;
            _categoryManager = categoryManager;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var allMovies = await _movieManager.GetPagedMoviesAsync(page, pageSize);
            return View(allMovies);
        }
        private async Task populatesDropDowns(IEnumerable<int> selectedActors = null, IEnumerable<int> selectedCinemas = null, IEnumerable<int> selectedProducers = null, IEnumerable<int> selectedCategories = null)
        {
            var actors = await _actorManager.GetAllActorsAsync();
            var categories = await _categoryManager.GetAllCategoriesAsync();
            var cinemas = await _cinemaManager.GetAllCinemasAsync();
            var producers = await _producerManager.GetAllProducersAsync();
            /////////////////////////////////////////////////////////////
            ViewBag.Actors = actors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.FullName,
                Selected = selectedActors?.Contains(a.Id) ?? false
            }).ToList();

            ViewBag.cinemas = cinemas.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = selectedCinemas?.Contains(a.Id) ?? false
            }).ToList();

            ViewBag.Producers = producers.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.FullName,
                Selected = selectedProducers?.Contains(a.Id) ?? false
            }).ToList();

            ViewBag.categories = categories.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = selectedCategories?.Contains(a.Id) ?? false
            }).ToList();

            ViewBag.Languages = EnumHelper.GetEnumSelectList<Language>();
            ViewBag.Translations = EnumHelper.GetEnumSelectList<TranslationType>();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await populatesDropDowns();
            return View(new CreateMovieDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieDto movie)
        {

            if (!ModelState.IsValid)
            {
                await populatesDropDowns(movie.ActorsIds, movie.CinemasIds, movie.CategoryIds);
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .Select(m => new
                    {
                        Key = m.Key,
                        Errors = m.Value.Errors.Select(e => e.ErrorMessage)
                    }).ToList();
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(errors));

                return View(movie);
            }
            string PosterImgName = await _imageServises.UploadImageAsync(movie.PosterImg, "Movies");
            string BackgroundImgName = await _imageServises.UploadImageAsync(movie.BackgroundImg, "Movies");
            var movieDto = new CreateMovieDto
            {
                PosterUrl = "/Images/Movies/" + PosterImgName,
                BackgroundUrl = "/Images/Movies/" + BackgroundImgName,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                CategoryIds = movie.CategoryIds,
                Language = movie.Language,
                Translation = movie.Translation,
                ActorsIds = new List<int>(),
                CinemasIds = new List<int>(),
                ProducerIds = new List<int>()

            };

            await _movieManager.CreateMovieAsync(movieDto);
            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieManager.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return View("NotFound");
            }
            return View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieManager.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return View("NotFound");
            }
            await populatesDropDowns();
            var Dto = new UpdateMovieDto
            {
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                CategoryIds = movie.CategoryIds,
                Language = movie.Language,
                Translation = movie.Translation,
                PosterUrl = movie.PosterUrl,
                BackgroundUrl = movie.BackgroundUrl,
                ActorsIds = movie.ActorsIds,
                CinemasIds = movie.CinemasIds,
                ProducerIds = movie.ProducerIds
            };
            return View(Dto);

        }


    }
}
