using Business.DTOs.Integration;
using Business.DTOs.Movies;
using Business.Managers.Actors;
using Business.Managers.Categories;
using Business.Managers.Cinemas;
using Business.Managers.Movies;
using Business.Managers.Producers;
using Business.Services.TmdbService;
using Core.Entities.Helpers;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieManager _movieManager;
        private readonly IActorsManager _actorManager;
        private readonly ICinemasManager _cinemaManager;
        private readonly IProducersManager _producerManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IImageService _imageService;
        private readonly ITmdbService _tmdbService;

        public MoviesController(IMovieManager movieManager, IActorsManager actorManager,
            ICinemasManager cinemaManager, IProducersManager producerManager,
            ICategoryManager categoryManager, IImageService imageService, ITmdbService tmdbService)
        {
            _movieManager = movieManager;
            _actorManager = actorManager;
            _cinemaManager = cinemaManager;
            _producerManager = producerManager;
            _categoryManager = categoryManager;
            _imageService = imageService;
            _tmdbService = tmdbService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var allMovies = await _movieManager.GetPagedMoviesAsync(page, pageSize);
            return View(allMovies);
        }

        private async Task populatesDropDowns(IEnumerable<int> selectedActors = null,
            IEnumerable<int> selectedCinemas = null, IEnumerable<int> selectedProducers = null,
            IEnumerable<int> selectedCategories = null)
        {
            var actors = await _actorManager.GetAllActorsAsync();
            var categories = await _categoryManager.GetAllCategoriesAsync();
            var cinemas = await _cinemaManager.GetAllCinemasAsync();
            var producers = await _producerManager.GetAllProducersAsync();

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMovieDto movie)
        {
            if (!ModelState.IsValid)
            {
                await populatesDropDowns(
                    selectedActors: movie.ActorsIds,
                    selectedProducers: movie.ProducerIds,
                    selectedCategories: movie.CategoryIds);
                return View(movie);
            }

            var movieDto = new CreateMovieDto
            {
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                CategoryIds = movie.CategoryIds,
                Language = movie.Language,
                Translation = movie.Translation,
                ActorsIds = movie.ActorsIds ?? new List<int>(),
                ProducerIds = movie.ProducerIds ?? new List<int>()
            };

            if (movie.PosterImg != null)
            {
                movieDto.PosterUrl = await _imageService.UploadImageAsync(movie.PosterImg, "Movies", ImageType.Poster);
            }
            else if (movie.PosterUrl != null)
            {
                movieDto.PosterUrl = movie.PosterUrl;
            }

            if (movie.BackgroundImg != null)
            {
                movieDto.BackgroundUrl = await _imageService.UploadImageAsync(movie.BackgroundImg, "Movies", ImageType.Background);
            }
            else if (movie.BackgroundUrl != null)
            {
                movieDto.BackgroundUrl = movie.BackgroundUrl;
            }

            await _movieManager.CreateMovieAsync(movieDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var movieDetails = await _tmdbService.GetMovieDetailsAsync(id);
                if (movieDetails == null)
                {
                    return View("NotFound");
                }
                return View(movieDetails);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Could not retrieve movie details at this time";
                return View("SearchTmdb");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return View("NotFound");

            var movie = await _movieManager.GetMovieByIdAsync(id);
            if (movie == null) return View("NotFound");

            await populatesDropDowns(movie.ActorsIds, movie.CinemasIds, movie.ProducersIds, movie.CategoryIds);
            var Dto = new UpdateMovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                CategoryIds = movie.CategoryIds,
                Language = movie.Language,
                Translation = movie.Translation,
                PosterUrl = movie.PosterUrl,
                BackgroundUrl = movie.BackgroundUrl,
                ActorsIds = movie.ActorsIds,
                ProducerIds = movie.ProducersIds
            };
            return View(Dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMovieDto dto)
        {
            if (dto.Id <= 0) return View("NotFound");

            if (!ModelState.IsValid)
            {
                await populatesDropDowns(
                    selectedActors: dto.ActorsIds,
                    selectedProducers: dto.ProducerIds,
                    selectedCategories: dto.CategoryIds);
                return View(dto);
            }

            var existingMovie = await _movieManager.GetMovieByIdAsync(dto.Id);
            if (existingMovie == null) return View("NotFound");

            if (dto.PosterImg != null)
            {
                if (!string.IsNullOrEmpty(dto.PosterUrl) && !existingMovie.PosterUrl.StartsWith("http"))
                {
                    await _imageService.DeleteImageAsync(existingMovie.PosterUrl);
                }
                dto.PosterUrl = await _imageService.UploadImageAsync(dto.PosterImg, "Movies", ImageType.Poster);
            }
            else if (!string.IsNullOrEmpty(dto.PosterUrl) && dto.PosterUrl != existingMovie.PosterUrl)
            {
                if (!string.IsNullOrEmpty(existingMovie.PosterUrl) && !existingMovie.PosterUrl.StartsWith("http"))
                {
                    await _imageService.DeleteImageAsync(existingMovie.PosterUrl);
                }
            }

            if (dto.BackgroundImg != null)
            {
                if (!string.IsNullOrEmpty(dto.BackgroundUrl) && !existingMovie.BackgroundUrl.StartsWith("http"))
                {
                    await _imageService.DeleteImageAsync(existingMovie.BackgroundUrl);
                }
                dto.BackgroundUrl = await _imageService.UploadImageAsync(dto.BackgroundImg, "Movies", ImageType.Background);
            }
            else if (!string.IsNullOrEmpty(dto.BackgroundUrl) && dto.BackgroundUrl != existingMovie.BackgroundUrl)
            {
                if (!string.IsNullOrEmpty(existingMovie.BackgroundUrl) && !existingMovie.BackgroundUrl.StartsWith("http"))
                {
                    await _imageService.DeleteImageAsync(existingMovie.BackgroundUrl);
                }
            }

            await _movieManager.UpdateMovieAsync(dto);
            return RedirectToAction("Index");
        }

        private static readonly Dictionary<string, int> GenreIds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Action", 28 },
            { "Adventure", 12 },
            { "Animation", 16 },
            { "Comedy", 35 },
            { "Crime", 80 },
            { "Documentary", 99 },
            { "Drama", 18 },
            { "Family", 10751 },
            { "Fantasy", 14 },
            { "History", 36 },
            { "Horror", 27 },
            { "Music", 10402 },
            { "Musical", 10402 },
            { "Mystery", 9648 },
            { "Romance", 10749 },
            { "Sci-Fi", 878 },
            { "SciFi", 878 },
            { "Thriller", 53 },
            { "War", 10752 },
            { "Western", 37 }
        };

        [HttpGet]
        public async Task<IActionResult> SearchTmdb(string query, string category, int page = 1)
        {
            var categories = await _categoryManager.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;

            int? genreId = null;
            if (!string.IsNullOrWhiteSpace(category) && GenreIds.TryGetValue(category, out var gId))
            {
                genreId = gId;
            }

            TmdbSearchResponse response = null;
            if (string.IsNullOrWhiteSpace(query))
            {
                if (genreId.HasValue)
                {
                    response = await _tmdbService.GetMoviesByGenreAsync(genreId.Value, page);
                    ViewBag.IsTrending = false;
                }
                else
                {
                    response = await _tmdbService.GetPopularMoviesAsync(page);
                    ViewBag.IsTrending = true;
                }
            }
            else
            {
                response = await _tmdbService.SearchMoviesAsync(query, page);
                ViewBag.IsTrending = false;

                if (genreId.HasValue && response?.Results != null)
                {
                    response.Results = response.Results.Where(m => m.Genre_Ids != null && m.Genre_Ids.Contains(genreId.Value)).ToList();
                }
            }

            ViewBag.SrechQuery = query;
            ViewBag.CurrentPage = page;
            if (response == null)
            {
                response = new TmdbSearchResponse
                {
                    Results = new List<TmdbMovieBasic>(),
                    Total_Pages = 0
                };
            }
            ViewBag.TotalPages = response.Total_Pages > 500 ? 500 : response.Total_Pages;
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> FilterCatalog(string? searchTerm, MovieCategory? category, int pageNumber = 1, int pageSize = 12)
        {
            var result = await _movieManager.GetFilteredMoviesAsync(searchTerm, category, pageNumber, pageSize);
            return Json(new { success = true, data = result });
        }
    }
}
