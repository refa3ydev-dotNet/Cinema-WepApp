using Business.DTOs.Cinemas;
using Business.DTOs.Rooms;
using Business.Managers.Agent;
using Business.Managers.Cinemas;
using Business.Managers.Rooms;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AgentController(UserManager<ApplicationUser> userManager, IAgentDashboardManager dashboardManager, ICinemasManager cinemaManager, IImageService imageService, IRoomManager roomManager)
        {
            _userManager = userManager;
            _dashboardManager = dashboardManager;
            _cinemaManager = cinemaManager;
            _imageService = imageService;
            _roomManager = roomManager;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
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
            var user = await _userManager.GetUserAsync(User);
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
            var user = await _userManager.GetUserAsync(User);
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
        public async Task<IActionResult> CreateRoom(CreateRoomDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var user = await _userManager.GetUserAsync(User);
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
            var user = await _userManager.GetUserAsync(User);
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
        public async Task<IActionResult> UpdateRoom(UpdateRoomDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
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
            await _roomManager.UpdateRoomAsync(dto);
            return RedirectToAction("Rooms");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var user = await _userManager.GetUserAsync(User);
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
    }
}
