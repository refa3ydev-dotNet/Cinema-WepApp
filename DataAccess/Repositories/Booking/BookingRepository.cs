using Core.Entities;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class BookingRepository:IBookingRepository
{
    private readonly MoviesDbContext _context;

    public BookingRepository(MoviesDbContext context)
    {
        _context = context;
    }
    public async Task<bool> AreSeatsAvailableAsync(int scheduleId, List<int> bookedSeatIds)
    {
        var bookedSeats = await _context.Bookings
            .Where(b => b.MovieScheduleId == scheduleId && b.Status == "Confirmed")
            .SelectMany(b => b.BookingSeats.Select(bs => bs.SeatId))
            .ToListAsync();
        
        return !bookedSeatIds.Any(id=>bookedSeats.Contains(id));
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<List<int>> GetBookedSeatIdsAsync(int scheduleId)
    {
        return await _context.Bookings
            .Where(b => b.MovieScheduleId == scheduleId && b.Status == "Confirmed")
            .SelectMany(b => b.BookingSeats.Select(bs => bs.SeatId))
            .ToListAsync();
    }

    public async Task<List<Booking>> GetUserBookingsAsync(string userId)
    {
        return await _context.Bookings
            .Include(b => b.MovieSchedule)
            .ThenInclude(ms => ms.Movie)
            .Include(b => b.MovieSchedule)
            .ThenInclude(ms => ms.Cinema)
            .Include(b => b.MovieSchedule)
            .ThenInclude(ms => ms.Room)
            .Include(b => b.BookingSeats)
            .ThenInclude(ms => ms.Seat)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }
}
