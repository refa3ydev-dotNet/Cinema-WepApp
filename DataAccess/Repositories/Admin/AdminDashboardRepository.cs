
using Business.DTOs.Admin;
using Core.Enums;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Admin
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly MoviesDbContext _context;
        public AdminDashboardRepository(MoviesDbContext context)
        {
            _context = context;
        }
        public async Task<DashboardSummaryResult> GetSummaryAsync()
        {
            var now = DateTime.Now;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var previousMonthStart = currentMonthStart.AddMonths(-1);

            var currentRevenue = await _context.Bookings
                .Where(b => b.Status == "Confirmed" && b.BookingDate >= currentMonthStart)
                .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;

            var previousRevenue = await _context.Bookings
                .Where(b =>
                    b.Status == "Confirmed" &&
                    b.BookingDate >= previousMonthStart &&
                    b.BookingDate < currentMonthStart)
                .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;

            var currentBookings = await _context.Bookings
                .CountAsync(b => b.Status == "Confirmed" && b.BookingDate >= currentMonthStart);

            var previousBookings = await _context.Bookings
                .CountAsync(b =>
                    b.Status == "Confirmed" &&
                    b.BookingDate >= previousMonthStart &&
                    b.BookingDate < currentMonthStart);

            return new DashboardSummaryResult
            {
                TotalRevenue = await _context.Bookings
                    .Where(b => b.Status == "Confirmed")
                    .SumAsync(b => (decimal?)b.TotalPrice) ?? 0,

                TotalBookings = await _context.Bookings
                    .CountAsync(b => b.Status == "Confirmed"),

                ActiveCinemas = await _context.Cinemas
                    .CountAsync(c =>
                        c.ApprovalStatus == ApprovalStatus.Approved &&
                        !c.IsDeleted),

                PendingApprovals = await _context.Cinemas
                    .CountAsync(c =>
                        c.ApprovalStatus == ApprovalStatus.Pending &&
                        !c.IsDeleted),

                RegisteredUsers = await _context.Users
                    .CountAsync(u => !u.IsDeleted),

                RevenueGrowthPercentage = previousRevenue == 0
                    ? 0
                    : Math.Round(((currentRevenue - previousRevenue) / previousRevenue) * 100, 1),

                BookingGrowthPercentage = previousBookings == 0
                    ? 0
                    : Math.Round(((decimal)(currentBookings - previousBookings) / previousBookings) * 100, 1)
            };
        }

        public async Task<List<ChartPointResult>> GetRevenueChartAsync(int days)
        {
            var startDate = DateTime.Now.Date.AddDays(-(days - 1));

            var groupedData = await _context.Bookings
                .Where(b =>
                    b.Status == "Confirmed" &&
                    b.BookingDate.Date >= startDate)
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Value = g.Sum(x => x.TotalPrice)
                })
                .ToListAsync();

            return Enumerable.Range(0, days)
                .Select(i =>
                {
                    var date = startDate.AddDays(i);
                    var row = groupedData.FirstOrDefault(x => x.Date == date);

                    return new ChartPointResult
                    {
                        Label = date.ToString(days <= 7 ? "ddd" : "MMM dd"),
                        Value = row?.Value ?? 0
                    };
                })
                .ToList();
        }

        public async Task<List<ChartPointResult>> GetBookingsChartAsync(int days)
        {
            var startDate = DateTime.Now.Date.AddDays(-(days - 1));

            var groupedData = await _context.Bookings
                .Where(b =>
                    b.Status == "Confirmed" &&
                    b.BookingDate.Date >= startDate)
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Value = g.Count()
                })
                .ToListAsync();

            return Enumerable.Range(0, days)
                .Select(i =>
                {
                    var date = startDate.AddDays(i);
                    var row = groupedData.FirstOrDefault(x => x.Date == date);

                    return new ChartPointResult
                    {
                        Label = date.ToString(days <= 7 ? "ddd" : "MMM dd"),
                        Value = row?.Value ?? 0
                    };
                })
                .ToList();
        }

        public async Task<List<PendingCinemaApprovalResult>> GetPendingCinemasAsync(int take = 5)
        {
            return await _context.Cinemas
                .Where(c =>
                    c.ApprovalStatus == ApprovalStatus.Pending &&
                    !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Take(take)
                .Select(c => new PendingCinemaApprovalResult
                {
                    CinemaId = c.Id,
                    CinemaName = c.Name,
                    Location = c.Address,
                    SubmittedAt = c.CreatedAt,
                    Status = c.ApprovalStatus.ToString(),
                    ImageUrl = c.Logo,

                    AgentName = _context.Users
                        .Where(u => u.CinemaId == c.Id && !u.IsDeleted)
                        .Select(u => ((u.FirstName ?? "") + " " + (u.LastName ?? "")).Trim())
                        .FirstOrDefault() ?? "No Agent"
                })
                .ToListAsync();
        }

        public async Task<List<TopMoviesDashboardResult>> GetTopMoviesAsync(int take = 5)
        {
            return await _context.Bookings
                .Where(b =>
                    b.Status == "Confirmed" &&
                    b.MovieSchedule != null &&
                    b.MovieSchedule.Movie != null)
                .GroupBy(b => new
                {
                    b.MovieSchedule.Movie.Id,
                    b.MovieSchedule.Movie.Name,
                    b.MovieSchedule.Movie.PosterImg,
                    b.MovieSchedule.Movie.Rating
                })
                .Select(g => new TopMoviesDashboardResult
                {
                    MovieId = g.Key.Id,
                    Title = g.Key.Name,
                    PosterUrl = g.Key.PosterImg,
                    Rating = (double)g.Key.Rating,
                    Revenue = g.Sum(b => b.TotalPrice),

                    // لو كل Booking يمثل تذكرة واحدة، استخدم Count()
                    // لو كل Booking ممكن يحتوي أكتر من Seat، استخدم BookingSeats.Count
                    TicketsSold = g.Sum(b => b.BookingSeats.Count)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<RecentActivityResult>> GetRecentActivitiesAsync(int take = 6)
        {
            var cinemaActivities = await _context.Cinemas
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Take(4)
                .Select(c => new RecentActivityResult
                {
                    Title = c.Name,
                    Description = c.ApprovalStatus == ApprovalStatus.Approved
                        ? "Cinema approved"
                        : c.ApprovalStatus == ApprovalStatus.Rejected
                            ? "Cinema rejected"
                            : "Cinema submitted for approval",

                    Type = c.ApprovalStatus == ApprovalStatus.Approved
                        ? "success"
                        : c.ApprovalStatus == ApprovalStatus.Rejected
                            ? "danger"
                            : "warning",

                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            var bookingActivities = await _context.Bookings
                .Where(b => b.Status == "Confirmed")
                .OrderByDescending(b => b.BookingDate)
                .Take(4)
                .Select(b => new RecentActivityResult
                {
                    Title = "New booking",
                    Description = $"Booking completed with total {b.TotalPrice}",
                    Type = "info",
                    CreatedAt = b.BookingDate
                })
                .ToListAsync();

            return cinemaActivities
                .Concat(bookingActivities)
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .ToList();
        }
    }
}

