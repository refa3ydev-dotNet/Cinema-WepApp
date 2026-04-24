using System.Security.Claims;
using Business.DTOs.Bookings;
using Business.Managers.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movies_web_app.Controllers;
[Authorize]
public class CustomerController:Controller
{
    private readonly IBookingManager _bookingManager;
    public CustomerController(IBookingManager bookingManager)
    {
        _bookingManager = bookingManager;
    }

    [HttpGet]
    public async Task<IActionResult> SelectSeats(int ScheduleId)
    {
        if (ScheduleId <= 0)
        {
            return BadRequest("Invalid Schedule ID.");
        }
        var seatSelectionDto=await _bookingManager.GetSeatSelectionDataAsync(ScheduleId);
        if (seatSelectionDto == null)
        {
            return NotFound("Schedule not found or is no longer active.");
        }
        return View(seatSelectionDto);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutDto checkoutDto)
    {
        if (string.IsNullOrEmpty(checkoutDto.SelectedSeatIds))
        {
            TempData["ErrorMessage"] = "Please select at least one seat before checking out.";
            return RedirectToAction("SelectSeats", new { scheduleId = checkoutDto.ScheduleId });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        bool isSuccess = await _bookingManager.ProcessCheckoutAsync(checkoutDto, userId);

        if (isSuccess)
        {
            TempData["SeccessMessage"] = "Tickets booked successfully! Grab your popcorn 🍿";
            return RedirectToAction("MyTickets");
            
        }
        TempData["ErrorMessage"] = "Sorry, one or more selected seats were just booked by someone else.";
        return RedirectToAction("SelectSeats", new { scheduleId = checkoutDto.ScheduleId });
    }
    [HttpGet]
    public async Task<IActionResult> MyTickets()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
            
        }

        var tickets = await _bookingManager.GetUserTicketsAsync(userId);
        tickets = tickets.OrderByDescending(t => t.StartTime > DateTime.Now)
            .ThenBy(t => t.StartTime)
            .ToList();
        return View(tickets);
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetMovieSchedules(int movieId)
    {
        if (movieId <= 0) return BadRequest();

        var schedules = await _bookingManager.GetAvailableSchedulesForMovieAsync(movieId);

        return Json(schedules);
    }
}