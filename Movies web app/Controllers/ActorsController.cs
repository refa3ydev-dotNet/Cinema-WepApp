using Business.DTOs.Actors;
using Business.Managers.Actors;
using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    public class ActorsController : Controller
    {
        private readonly MoviesDbContext _context;
        private readonly IActorsManager _actorsmanager;
        private readonly IImageService _imageServises;


        public ActorsController(MoviesDbContext context, IActorsManager actorsmanager, IImageService imageServises)
        {
            _context = context;
            _actorsmanager = actorsmanager;
            _imageServises = imageServises;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            Console.WriteLine($"Search Term Received: '{searchString}'");
            ViewData["CurrentFilter"] = searchString;
            List<GetAllActorsDto> actors;

            if (!string.IsNullOrEmpty(searchString))
            {
                actors = await _actorsmanager.SearchActorsAsync(searchString);
                Console.WriteLine($"Found {actors.Count} actors.");
            }
            else
            {
                actors = await _actorsmanager.GetAllActorsAsync();
                Console.WriteLine("Fetching All Actors.");

            }
            return View(actors);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateActorDto actor)
        {
            Console.WriteLine("Create Action Hit ✅");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model is invalid ❌");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }

                return View(actor);
            }
            var act = new CreateActorDto
            {
                FullName = actor.FullName,
                Bio = actor.Bio,
                IMDBLink = actor.IMDBLink,
                BirthDate = actor.BirthDate,
                DeathDate = actor.DeathDate,
                Nationality = actor.Nationality,


        };
            if (act.ProfilePicture != null)
            {
                string imageName = await _imageServises.UploadImageAsync(act.ProfilePicture, "Actors");
                act.ProfilePath = "/Images/Actors/" + imageName;

            }
            if (actor.ProfilePicture == null)
            {
                if (actor.ProfilePath != null)
                {
                    act.ProfilePath = actor.ProfilePath;
                }
            }

        await _actorsmanager.CreateActorAsync(act);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _actorsmanager.GetActorByIdAsync(id);
            if (actor == null) return View("NotFound");
            if (!string.IsNullOrEmpty(actor.ProfilePath))
            {
                var relativePath = Path.Combine("wwwroot", actor.ProfilePath);
                Console.WriteLine("Path: " + actor.ProfilePath);

                await _imageServises.DeleteImageAsync(relativePath);
            }

            await _actorsmanager.DeleteActorAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            var actor = await _actorsmanager.GetActorByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _actorsmanager.GetActorByIdAsync(id);
            if (actor == null) return View("NotFound");
            var dto = new UpdateActorDto
            {
                Id = actor.Id,
                FullName = actor.FullName,
                Bio = actor.Bio,
                ProfilePath = actor.ProfilePath,
                IMDBLink = actor.IMDBLink,
                BirthDate = actor.BirthDate
            };
            return View(dto);
        }
        [HttpPost]
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
                    await _imageServises.DeleteImageAsync(dto.ProfilePath);
                }
                string imageName = await _imageServises.UploadImageAsync(dto.ProfilePicture, "Actors");
                dto.ProfilePath = "/Images/Actors/" + imageName;
            }
            await _actorsmanager.UpdateActorAsync(dto);

            return RedirectToAction("Details", new { id = dto.Id });
        }
    }
}
