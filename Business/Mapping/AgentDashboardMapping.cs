using Business.DTOs.Agent;
using Core;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Mapping
{
    public static class AgentDashboardMapping
    {
        public static RecentBookingDto ToDto(this Booking booking)
        {
            return new RecentBookingDto()
            {
                MovieName = booking.MovieSchedule?.Movie?.Name ?? "Unknown Movie",
                MoviePoster = booking.MovieSchedule?.Movie?.PosterImg ?? "",
                ScheduleTime = booking.MovieSchedule?.StartDate ?? DateTime.MinValue,
                RoomName = booking.MovieSchedule?.Room?.RoomName ?? "Unknown Room",
                CustomerName = booking.User != null ? $"{booking.User.FirstName} {booking.User.LastName}" : "Unknown Customer",
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                SeatInfo = booking.BookingSeats != null
                ? string.Join(", ", booking.BookingSeats.Select(bs => $"{bs.Seat.Row}{bs.Seat.Column}")) : "",

            };
        }

        public static AgentDashboardDto ToDto(string agentName, Cinema cinema,
            List<Booking> todaysBookings, int activeMoviesCount, List<Booking> recentBookings)
        {
            return new AgentDashboardDto()
            {
                AgentName = agentName,
                CinemaName = cinema?.Name??"Unknown Cinema",
                TicketSoldToday = todaysBookings?.Sum(b => b.BookingSeats?.Count ?? 0) ?? 0,
                DailyRevenue = todaysBookings?.Sum(b => b.TotalPrice) ?? 0,
                ActiveMoviesCount = activeMoviesCount,
                RecentBookings = recentBookings?.Select(b => b.ToDto()).ToList() ?? new List<RecentBookingDto>()
            };
        }
    }
}
