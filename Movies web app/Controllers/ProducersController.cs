using Business.DTOs.Producers;
using Business.Managers.Producers;
using Core.Enums;
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
            
            if (!ModelState.IsValid)
            {
                
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
                Producer.ProfilePath =
                    await _imageServises.UploadImageAsync(Producer.ProfilePicture, "Producers", ImageType.Profile);
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

                await _imageServises.DeleteImageAsync(Producer.ProfilePath);
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
                Id = Producer.Id,
                FullName = Producer.FullName,
                Bio = Producer.Bio,
                ProfilePath = Producer.ProfilePath,
                IMDBLink = Producer.IMDBLink,
                BirthDate = Producer.BirthDate,
                DeathDate = Producer.DeathDate,
                Nationality = Producer.Nationality
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
                dto.ProfilePath = 
                        await _imageServises.UploadImageAsync(dto.ProfilePicture, "Producers", ImageType.Profile);
            }
            await _producersManager.UpdateProducerAsync(dto);

            return RedirectToAction("Details", new { id = dto.Id });
        }
    }
}
