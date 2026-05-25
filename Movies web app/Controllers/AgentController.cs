using Business.DTOs.Cinemas;
using Business.DTOs.Rooms;
using Business.DTOs.Schedule;
using Business.Managers.Agent;
using Business.Managers.Cinemas;
using Business.Managers.Movies;
using Business.Managers.Rooms;
using Business.Managers.Schedule;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movies_web_app.Services;

namespace Movies_web_app.Controllers
{
    [Authorize(Roles = "CinemaAgent")]
    public class AgentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAgentDashboardManager _dashboardManager;
        private readonly ICinemasManager _cinemaManager;
        private readonly IImageService _imageService;
        private readonly IRoomManager _roomManager;
        private readonly IMovieManager _movieManager;
        private readonly IMovieScheduleManager _movieScheduleManager;


public AgentController(UserManager<ApplicationUser> userManager,
    IAgentDashboardManager dashboardManager,
    ICinemasManager cinemaManager,
    IImageService imageService,
    IRoomManager roomManager,
    IMovieManager movieManager,
    IMovieScheduleManager scheduleManager
    )
{
    _userManager = userManager;
    _dashboardManager = dashboardManager;
    _cinemaManager = cinemaManager;
    _imageService = imageService;
    _roomManager = roomManager;
    _movieManager = movieManager;
    _movieScheduleManager = scheduleManager;


}

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cinema = await _cinemaManager.GetCinemaByIdAsync(user.CinemaId.Value);
            if (cinema == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (cinema.ApprovalStatus == ApprovalStatus.Pending)
            {
                return RedirectToAction("PendingApproval");
            }
            if (cinema.ApprovalStatus == ApprovalStatus.Rejected)
            {
                return RedirectToAction("FixApplication");
            }
            var dashboardDto = await _dashboardManager.GetAgentDashboardDataAsync(user.CinemaId.Value, user.FirstName);

            return View(dashboardDto);
        }

        [HttpGet]
        public IActionResult PendingApproval()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> FixApplication()
        {
            var user = await GetValidAgentAsync();
            var cinema = await _cinemaManager.GetCinemaByIdAsync(user.CinemaId.Value);
            if (user == null || cinema.ApprovalStatus != ApprovalStatus.Rejected)
            {
                return RedirectToAction("Dashboard");
            }
            var Dto = new FixCinemaApplicationDto
            {
                Id = user.CinemaId.Value,
                Name = cinema.Name,
                Address = cinema.Address,
                Description = cinema.Description,
                LogoPath = cinema.LogoPath,
                BackgroundPicturePath = cinema.BackgroundPath,
                RejectionReason = cinema.RejectionReason,
                ApprovalStatus = cinema.ApprovalStatus

            };
            return View(Dto);
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> FixApplication(FixCinemaApplicationDto dto)
{
    if (!ModelState.IsValid)
    {
        return View(dto);
    }
    var existingCinema = await _cinemaManager.GetCinemaByIdAsync(dto.Id);
    if (existingCinema == null) return View("NotFound");
    if (dto.NewLogo != null)
    {
        if (!string.IsNullOrEmpty(dto.LogoPath) && !existingCinema.LogoPath.StartsWith("http"))
        {
            await _imageService.DeleteImageAsync(existingCinema.LogoPath);
        }

        dto.LogoPath = 
        await _imageService.UploadImageAsync(dto.NewLogo, "Cinemas", ImageType.Profile);


        }
        else if (!string.IsNullOrEmpty(dto.LogoPath) && dto.LogoPath != existingCinema.LogoPath)
        {
        if (!existingCinema.LogoPath.StartsWith("http"))
        {
            await _imageService.DeleteImageAsync(existingCinema.LogoPath);
        }
        }

        if (dto.NewBackground != null)
        {
        if (!string.IsNullOrEmpty(existingCinema.BackgroundPath) && !existingCinema.BackgroundPath.StartsWith("http"))
        {
            await _imageService.DeleteImageAsync(existingCinema.BackgroundPath);
        }

        dto.BackgroundPicturePath = 
        await _imageService.UploadImageAsync(dto.NewBackground, "Cinemas", ImageType.Background);

        }
        else if (!string.IsNullOrEmpty(dto.BackgroundPicturePath) && dto.BackgroundPicturePath != existingCinema.BackgroundPath)
        {
        if (!existingCinema.BackgroundPath.StartsWith("http"))
        {
            await _imageService.DeleteImageAsync(existingCinema.BackgroundPath);
        }
        }
        dto.ApprovalStatus = ApprovalStatus.Pending;

        await _cinemaManager.UpdateCinemaAsync(dto);
        return RedirectToAction("Dashboard");
}
        [HttpGet]
        public async Task<IActionResult> Rooms()
        {
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("login", "Account");
            }
            var rooms = await _roomManager.GetCinemaRoomsAsync(user.CinemaId.Value);
            return View(rooms);
        }
        [HttpGet]
        public IActionResult CreateRoom()
        {
            return View(new CreateRoomDto());
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CreateRoom(CreateRoomDto dto)
{
    if (!ModelState.IsValid)
    {
        return View(dto);
    }
    var user = await GetValidAgentAsync();
    if (user == null || user.CinemaId == null)
    {
        return RedirectToAction("Login", "Account");
    }
    await _roomManager.AddRoomAsync(dto, user.CinemaId.Value);
    return RedirectToAction("Rooms");
}
        [HttpGet]
        public async Task<IActionResult> UpdateRoom(int id)
        {
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var room = await _roomManager.GetRoomByIdAsync(id);
            if (room == null || room.CinemaId != user.CinemaId || room.IsDeleted) return View("NotFound");

            var dto = new UpdateRoomDto
            {
                Id = id,
                RoomName = room.RoomName,
                SeatCount = room.SeatCount,
                SeatsPerRow = room.SeatsPerRow
            };
            return View(dto);
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateRoom(UpdateRoomDto dto)
{
    var user = await GetValidAgentAsync();
    if (user == null || user.CinemaId == null)
    {
        return RedirectToAction("Login", "Account");
    }
    if (!ModelState.IsValid)
    {
        return View(dto);
    }

    var existingRoom = await _roomManager.GetRoomByIdAsync(dto.Id);
    if (existingRoom == null || existingRoom.CinemaId != user.CinemaId.Value || existingRoom.IsDeleted)
    {
        return View("NotFound");
    }
    bool isUpdated = await _roomManager.UpdateRoomAsync(dto);
    if (!isUpdated)
    {
        TempData["ErrorMassage"] = "Cannot update seats! This room has active upcoming schedules.";
        return View(dto);
    }
    TempData["SuccessMassage"] = "Room updated successfully!";
    return RedirectToAction("Rooms");
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteRoom(int id)
{
    var user = await GetValidAgentAsync();
    if (user == null || user.CinemaId == null)
    {
        return RedirectToAction("Login", "Account");
    }
    var existingRoom = await _roomManager.GetRoomByIdAsync(id);

    if (existingRoom == null || existingRoom.CinemaId != user.CinemaId.Value || existingRoom.IsDeleted)
    {
        return RedirectToAction("Rooms");
    }
    bool isDeleted = await _roomManager.DeleteRoomAsync(id);
    if (!isDeleted)
    {
        TempData["ErrorMassage"] = "Cannot delete this room! There are active future schedules and bookings linked to it. Cancel the schedules first.";
        return RedirectToAction("Rooms");
    }
    TempData["SuccessMessage"] = "Room deleted successfully";

    return RedirectToAction("Rooms");
}
        [HttpGet]
        public async Task<IActionResult> SelectMoviesForCinema()
        {
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var allMovies = await _movieManager.GetAllMoviesAsync();
            ViewBag.AvailableMovies = new SelectList(allMovies, "Id", "Name");
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> SelectMoviesForCinema(int movieId)
        {
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (movieId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid movie selected";
                return RedirectToAction("SelectMoviesForCinema");
            }
            await _movieManager.AssignMovieToCinemaAsync(movieId, user.CinemaId.Value);
            TempData["SuccessMessage"] = "Movie added to your cinema successfully!";
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public async Task<IActionResult> AgentMovies()
        {
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var movies = await _movieManager.GetMoviesByCinemaIdAsync(user.CinemaId.Value);
            var rooms = await _roomManager.GetCinemaRoomsAsync(user.CinemaId.Value);
            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomName");
            var newMoviesThisMonth=movies.Count(m=>m.CreatedAt.Month==DateTime.Now.Month&&m.CreatedAt.Year==DateTime.Now.Year);
            //var activeSchedulesCount = await _movieScheduleManager.GetActiveSchedulesCountByCinemaIdAsync(user.CinemaId.Value);
            // var ticketsSold = await _ticketManager.GetTicketsSoldCountByCinemaIdAsync(user.CinemaId.Value);
            ViewBag.NewMoviesThisMonth = newMoviesThisMonth;
            ViewBag.ActiveSchedules=0;
            ViewBag.TicketsSold=0;
            ViewBag.TotalMovies = movies.Count();
            return View(movies);
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> RemoveMovieFromCinema(int movieId)
{
    var user = await GetValidAgentAsync();
    if (user == null || user.CinemaId == null)
    {
        return RedirectToAction("Login", "Account");
    }
    bool isDeleted = await _movieManager.RemoveMovieFromCinemaAsync(movieId, user.CinemaId.Value);
    if (isDeleted)
    {
        TempData["SuccessMessage"] = "Movie removed from your cinema successfully!";

    }
    else
    {
        TempData["ErrorMessage"] = "Cannot remove this movie! There are active future schedules and bookings linked to it. Cancel the schedules first.";

    }
    return RedirectToAction("AgentMovies");
}
        [HttpGet]
        public async Task<IActionResult> CreateSchedule(int movieId)
        {
            if (movieId <= 0)
            {
                TempData["ErrorMessage"] = "Please select a valid movie first!";
                return RedirectToAction("AgentMovies");
            }
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null) return RedirectToAction("Login", "Account");

            var rooms = await _roomManager.GetCinemaRoomsAsync(user.CinemaId.Value);
            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomName"); 


            var movie = await _movieManager.GetMovieByIdAsync(movieId);
            ViewBag.MovieName = movie?.Name ?? "Unknown Movie";

            var dto = new CreateScheduleDto
            {
                MovieId = movieId,
                CinemaId = user.CinemaId.Value,
                StartTime = DateTime.Now.AddDays(1).Date.AddHours(18), // Default to tomorrow at 6 PM
                Price = 15
            };
            return View(dto);
        }
[HttpPost]
public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleDto dto)
{
    var user = await GetValidAgentAsync();

    if (user == null || !user.CinemaId.HasValue)
    {
        return Unauthorized(new { message = "You must be logged in and assigned to a cinema." });
    }

    ModelState.Remove("CinemaId");

    if (!ModelState.IsValid)
    {
        var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(new { message = $"Validation Error: {errors}" });
    }

    try
    {
        dto.CinemaId = user.CinemaId.Value;
        dto.StartTime = DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Local);
        
        await _movieScheduleManager.CreateScheduleAsync(dto);

        return Ok(new { success = true, message = "Schedule created successfully!" });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { success = false, message = "Server Error: " + ex.Message });
    }
}
        [HttpPost]
        public async Task<IActionResult> AddMovieToCinemaCatalog([FromForm] int tmdbId)
        {
            try
            {
                var user = await GetValidAgentAsync();

                if (user == null) return RedirectToAction("Login", "Account");
                await _movieManager.SyncMovieFromTmdbAsync(tmdbId, user.CinemaId.Value);
                return Json(new { success = true , message = $"Movie added to your cinema successfully!" });
            }catch(Exception ex)
            {
                string exactError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                // بنبعتها للـ JS عشان تظهرلك في الشاشة
                return StatusCode(500, new { success = false, message = "DB Error: " + exactError });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Schedules()
        {
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null) return RedirectToAction("Login", "Account");
            var rooms=await _roomManager.GetCinemaRoomsAsync(user.CinemaId.Value);
            ViewBag.Rooms=new SelectList(rooms, "Id", "RoomName");
            var schedule = await _movieScheduleManager.GetCinemaSchedulesAsync(user.CinemaId.Value);
            if (schedule == null) return View("NotFound");
            return View(schedule);
        }
        [HttpPost]
        public async Task<IActionResult> EditSchedule([FromBody] UpdateScheduleDto dto)
        {
            var user = await GetValidAgentAsync();
            if (user == null || user.CinemaId == null) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                try
                {
                    dto.CinemaId=user.CinemaId.Value;
                    dto.UpdatedAt = DateTime.Now;
                    await _movieScheduleManager.UpdateScheduleAsync(dto);
                    return Json(new { success = true, message = "Screening updated successfully!" });
                }
                catch(Exception ex)
                {
                    return Json(new { success = false, message = "Error: "+ex.Message });
                }
            }
            var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
          
            return Json(new { success = false, message = "Validation Error: " + errors });
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteSchedule(int id)
{
    var user = await GetValidAgentAsync();
    if (user == null || user.CinemaId == null) return RedirectToAction("Login", "Account");
    bool isDeleted = await _movieScheduleManager.DeleteScheduleAsync(id,user.CinemaId.Value);
    if (isDeleted)
    {
        return Json(new { success = true, message = "Schedule deleted successfully" });
    }
    return Json(new { success = false, message = "Cannot delete this schedule! There are active bookings linked to it. Cancel the bookings first." });
}
        private async Task<ApplicationUser?> GetValidAgentAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CinemaId == null)
            {
                return null;
            }
            return user;
        }
    }
}
