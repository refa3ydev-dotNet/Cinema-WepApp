using Business.DTOs.Producers;
using Business.Managers.Producers;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    public class ProducersController : Controller
    {
        private readonly IProducersManager _producersManager;
        private readonly IImageService _imageService;

        public ProducersController(IProducersManager producersManager, IImageService imageService)
        {
            _producersManager = producersManager;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var producers = await _producersManager.GetAllProducersAsync();
            return View(producers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProducerDto producer)
        {
            if (!ModelState.IsValid)
            {
                return View(producer);
            }

            if (producer.ProfilePicture != null)
            {
                producer.ProfilePath = await _imageService.UploadImageAsync(producer.ProfilePicture, "Producers", ImageType.Profile);
            }

            await _producersManager.CreateProducerAsync(producer);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var producer = await _producersManager.GetProducerByIdAsync(id);
            if (producer == null) return View("NotFound");

            if (!string.IsNullOrEmpty(producer.ProfilePath))
            {
                await _imageService.DeleteImageAsync(producer.ProfilePath);
            }

            await _producersManager.DeleteProducerAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var producer = await _producersManager.GetProducerByIdAsync(id);
            if (producer == null) return View("NotFound");
            return View(producer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producer = await _producersManager.GetProducerByIdAsync(id);
            if (producer == null) return View("NotFound");

            var dto = new UpdateProducerDto
            {
                Id = producer.Id,
                FullName = producer.FullName,
                Bio = producer.Bio,
                ProfilePath = producer.ProfilePath,
                IMDBLink = producer.IMDBLink,
                BirthDate = producer.BirthDate,
                DeathDate = producer.DeathDate,
                Nationality = producer.Nationality
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    await _imageService.DeleteImageAsync(dto.ProfilePath);
                }
                dto.ProfilePath = await _imageService.UploadImageAsync(dto.ProfilePicture, "Producers", ImageType.Profile);
            }

            await _producersManager.UpdateProducerAsync(dto);
            return RedirectToAction("Details", new { id = dto.Id });
        }
    }
}
