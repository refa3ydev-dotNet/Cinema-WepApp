using Core.Entities;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Dashboard
{
    public class AgentDashboardRepository : IAgentDashboardRepository
    {
        private readonly MoviesDbContext _context;
        public AgentDashboardRepository(MoviesDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetActiveMoviesCountAsync(int cinemaId)
        {
            return await _context.CinemaMovies.CountAsync(cm=>cm.CinemaId==cinemaId);
        }

        public async Task<List<Booking>> GetRecentBookingAsync(int cinemaId, int count)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.MovieSchedule).ThenInclude(ms => ms.Movie)
                .Include(b => b.MovieSchedule).ThenInclude(ms => ms.Room)
                .Include(b => b.BookingSeats).ThenInclude(bs => bs.Seat)
                .Where(b => b.MovieSchedule.Room.CinemaId == cinemaId)
                .OrderByDescending(b => b.BookingDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetTodayBookingsAsync(int cinemaId)
        {
            var toDay = DateTime.Today;
            return await _context.Bookings
                .Include(b=>b.BookingSeats)
                .Where(b => b.BookingDate.Date == toDay && b.MovieSchedule.Room.CinemaId == cinemaId)
                .ToListAsync();
        }
    }
}
