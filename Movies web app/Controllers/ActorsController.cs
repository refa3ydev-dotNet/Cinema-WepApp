using Business.DTOs.Actors;
using Business.Managers.Actors;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorsManager _actorsManager;
        private readonly IImageService _imageService;

        public ActorsController(IActorsManager actorsManager, IImageService imageService)
        {
            _actorsManager = actorsManager;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            List<GetAllActorsDto> actors;

            if (!string.IsNullOrEmpty(searchString))
            {
                actors = await _actorsManager.SearchActorsAsync(searchString);
            }
            else
            {
                actors = await _actorsManager.GetAllActorsAsync();
            }
            return View(actors);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateActorDto actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            if (actor.ProfilePicture != null)
            {
                actor.ProfilePath = await _imageService.UploadImageAsync(actor.ProfilePicture, "Actors", ImageType.Profile);
            }

            await _actorsManager.CreateActorAsync(actor);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _actorsManager.GetActorByIdAsync(id);
            if (actor == null) return View("NotFound");

            if (!string.IsNullOrEmpty(actor.ProfilePath))
            {
                await _imageService.DeleteImageAsync(actor.ProfilePath);
            }

            await _actorsManager.DeleteActorAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var actor = await _actorsManager.GetActorByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _actorsManager.GetActorByIdAsync(id);
            if (actor == null) return View("NotFound");

            var dto = new UpdateActorDto
            {
                Id = actor.Id,
                FullName = actor.FullName,
                Bio = actor.Bio,
                ProfilePath = actor.ProfilePath,
                IMDBLink = actor.IMDBLink,
                BirthDate = actor.BirthDate,
                DeathDate = actor.DeathDate,
                Nationality = actor.Nationality
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateActorDto dto)
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
                dto.ProfilePath = await _imageService.UploadImageAsync(dto.ProfilePicture, "Actors", ImageType.Profile);
            }

            await _actorsManager.UpdateActorAsync(dto);
            return RedirectToAction("Details", new { id = dto.Id });
        }
    }
}
