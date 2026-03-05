using Business.DTOs.Cinemas;
using Business.Managers.Accounts;
using Business.Managers.Actors;
using Business.Managers.Cinemas;
using Core.Enums;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Services;
using System.Security.Claims;

namespace Movies_web_app.Controllers
{
    public class CinemasController : Controller
    {
        private readonly MoviesDbContext _context;
        private readonly IAccountManager _accountmanager;
        private readonly ICinemasManager _cinemaManager;
        private readonly IImageService _imageServises;



        public CinemasController(MoviesDbContext context, ICinemasManager cinemaManager, IImageService imageServices, IAccountManager accountManager)
        {
            _context = context;
            _cinemaManager = cinemaManager;
            _imageServises = imageServices;
                _accountmanager = accountManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var allCinemas = await _cinemaManager.GetPagedCinemasAsync(page, pageSize);
            return View(allCinemas);
        }
        [Authorize(Roles = "CinemaAgent")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
           var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _accountmanager.GetUserByEmailAsync(userEmail);


            if (currentUser != null && currentUser.CinemaId != null)
            {
                return RedirectToAction("Details", "Cinemas", new { id = currentUser.CinemaId });
            }
            
            return View();
        }
        [Authorize(Roles = "CinemaAgent")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCinemaDto cinema)
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser=await _accountmanager.GetUserByEmailAsync(userEmail);
            if (currentUser != null && currentUser.CinemaId != null && currentUser.CinemaId != 0)
            {

                return RedirectToAction("Details", "Cinemas", new { id = currentUser.CinemaId });
            }
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            string logoPath = cinema.LogoPath;
            if(cinema.Logo != null)
            {
                logoPath =
                    await _imageServises.UploadImageAsync(cinema.Logo, "Cinemas",ImageType.Profile);

            }
            string backgroundPath = cinema.BackgroundPath;
            if (cinema.BackgroundPicture != null)
            {
                backgroundPath =
                    await _imageServises.UploadImageAsync(cinema.BackgroundPicture, "Cinemas",ImageType.Background);

            }
            var cinemaModel = new CreateCinemaDto
            {
                Name = cinema.Name,
                Description = cinema.Description,
                Address = cinema.Address,
                LogoPath = logoPath,
                BackgroundPath = backgroundPath

            };
            var newCinemaId = await _cinemaManager.CreateCinemaAsync(cinemaModel);

            if(currentUser!=null&& newCinemaId>0)
            {
                currentUser.CinemaId = newCinemaId;
                await _accountmanager.UpdateUserAsync(currentUser);
            }
           
            return RedirectToAction("PendingApproval");
        }
        [Authorize(Roles = "CinemaAgent")]
        [HttpGet]
        public async Task<IActionResult> PendingApproval()
        {
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            if(id<=0) return View("NotFound");
            var cinema = await _cinemaManager.GetCinemaByIdAsync(id);
            if (cinema == null) return View("NotFound");
            return View(cinema);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cinema = await _cinemaManager.GetCinemaByIdAsync(id);
            if (cinema == null) return View("NotFound");
            var dto = new UpdateCinemaDto
            {
                Id = cinema.Id,
                LogoPath = cinema.LogoPath,
                Name = cinema.Name,
                Description = cinema.Description,
                BackgroundPath = cinema.BackgroundPath,
                Address = cinema.Address
            };
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCinemaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var exsistingCinema = await _cinemaManager.GetCinemaByIdAsync(dto.Id);
            if (exsistingCinema == null) return View("NotFound");
            if (dto.Logo != null)
            {
                if (!string.IsNullOrEmpty(dto.LogoPath) && !exsistingCinema.LogoPath.StartsWith("http"))
                {
                    await _imageServises.DeleteImageAsync(exsistingCinema.LogoPath);
                }

                dto.LogoPath =
                await _imageServises.UploadImageAsync(dto.Logo, "Cinemas", ImageType.Profile);


            }
            else if(!string.IsNullOrEmpty(dto.LogoPath ) && dto.LogoPath != exsistingCinema.LogoPath)
            {
                if(!string.IsNullOrEmpty(exsistingCinema.LogoPath) && !exsistingCinema.LogoPath.StartsWith("http"))
                {
                    await _imageServises.DeleteImageAsync(exsistingCinema.LogoPath);
                }
            }

            if(dto.BackgroundPicture != null)
            {
                if (!string.IsNullOrEmpty(exsistingCinema.BackgroundPath) && !exsistingCinema.BackgroundPath.StartsWith("http"))
                {
                    await _imageServises.DeleteImageAsync(exsistingCinema.BackgroundPath);
                }

                    dto.BackgroundPath =
                    await _imageServises.UploadImageAsync(dto.BackgroundPicture, "Cinemas", ImageType.Background);

            }
            else if(!string.IsNullOrEmpty(dto.BackgroundPath) && dto.BackgroundPath != exsistingCinema.BackgroundPath)
            {
                if (!string.IsNullOrEmpty(exsistingCinema.BackgroundPath) && !exsistingCinema.BackgroundPath.StartsWith("http"))
                {
                    await _imageServises.DeleteImageAsync(exsistingCinema.BackgroundPath);
                }
            }


            await _cinemaManager.UpdateCinemaAsync(dto);
            return RedirectToAction("Details", new { id = dto.Id });

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _cinemaManager.GetCinemaByIdAsync(id);
            if (cinema == null) return View("NotFound");
            if (!string.IsNullOrEmpty(cinema.LogoPath))
            {
                await _imageServises.DeleteImageAsync(cinema.LogoPath);
            }
            if (!string.IsNullOrEmpty(cinema.BackgroundPath))
            {
                await _imageServises.DeleteImageAsync(cinema.BackgroundPath);
            }
            await _cinemaManager.DeleteCinemaAsync(id);
            return RedirectToAction("Index");
        }

    }
}
