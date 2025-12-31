using Business.DTOs.Producers;
using Business.Managers.Producers;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    public class ProducersController : Controller
    {
        private readonly MoviesDbContext _context;
        private readonly IProducersManager _producersManager;
        private readonly IImageService _imageServises;

        public ProducersController(MoviesDbContext context, IProducersManager producersmanager, IImageService imageServises)
        {
            _context = context;
            _producersManager = producersmanager;
            _imageServises = imageServises;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var Producers = await _producersManager.GetAllProducersAsync();
            return View(Producers);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProducerDto Producer)
        {
            Console.WriteLine("Create Action Hit ✅");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model is invalid ❌");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error in '{state.Key}': {error.ErrorMessage}");
                    }
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine("Validation error: " + modelError.ErrorMessage);
                    }

                }
                return View(Producer);
            }
            if (Producer.ProfilePicture != null)
            {
                string imageName = await _imageServises.UploadImageAsync(Producer.ProfilePicture, "Producers");
                Producer.ProfilePath = "/Images/Producers/" + imageName;
            }
            await _producersManager.CreateProducerAsync(Producer);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var Producer = await _producersManager.GetProducerByIdAsync(id);
            if (Producer == null) return View("NotFound");
            if (!string.IsNullOrEmpty(Producer.ProfilePath))
            {
                var relativePath = Path.Combine("wwwroot", Producer.ProfilePath);
                Console.WriteLine("Path: " + Producer.ProfilePicture);

                await _imageServises.DeleteImageAsync(relativePath);
            }

            await _producersManager.DeleteProducerAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            var Producer = await _producersManager.GetProducerByIdAsync(id);
            if (Producer == null) return View("NotFound");
            return View(Producer);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var Producer = await _producersManager.GetProducerByIdAsync(id);
            if (Producer == null) return View("NotFound");
            var dto = new UpdateProducerDto
            {
                FullName = Producer.FullName,
                Bio = Producer.Bio,
                ProfilePath = Producer.ProfilePath
            };
            Console.WriteLine(Producer.ProfilePicture);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProducerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            if (dto.ProfilePicture != null)
            {
                if (!string.IsNullOrEmpty(dto.ProfilePath))
                {
                    await _imageServises.DeleteImageAsync(dto.ProfilePath);
                }
                string imageName = await _imageServises.UploadImageAsync(dto.ProfilePicture, "Producers");
                dto.ProfilePath = "/Images/Producers/" + imageName;
            }
            await _producersManager.UpdateProducerAsync(dto);

            return RedirectToAction("Details", new { id = dto.Id });
        }
    }
}
