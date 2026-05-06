using Business.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Mapping
{
    public static class AdminDashboardMapper
    {

        public static DashboardSummaryDto ToDto(this DashboardSummaryResult data)
        {
            return new DashboardSummaryDto
            {
                TotalRevenue = data.TotalRevenue,
                TotalBookings = data.TotalBookings,
                ActiveCinemas = data.ActiveCinemas,
                PendingApprovals = data.PendingApprovals,
                RegisteredUsers = data.RegisteredUsers,
                RevenueGrowthPercentage = data.RevenueGrowthPercentage,
                BookingGrowthPercentage = data.BookingGrowthPercentage
            };
        }

        public static ChartPointDto ToDto(this ChartPointResult data)
        {
            return new ChartPointDto
            {
                Label = data.Label,
                Value = data.Value
            };
        }

        public static PendingCinemaApprovalDto ToDto(this PendingCinemaApprovalResult data)
        {
            return new PendingCinemaApprovalDto
            {
                CinemaId = data.CinemaId,
                CinemaName = data.CinemaName,
                AgentName = data.AgentName,
                Location = data.Location,
                SubmittedAt = data.SubmittedAt,
                Status = data.Status,
                ImageUrl = data.ImageUrl
            };
        }

        public static TopMoviesDashboardDto ToDto(this TopMoviesDashboardResult data)
        {
            return new TopMoviesDashboardDto
            {
                MovieId = data.MovieId,
                Title = data.Title,
                PosterUrl = data.PosterUrl,
                TicketsSold = data.TicketsSold,
                Revenue = data.Revenue,
                Rating = data.Rating
            };
        }

        public static RecentActivityDto ToDto(this RecentActivityResult data)
        {
            return new RecentActivityDto
            {
                Title = data.Title,
                Description = data.Description,
                Type = data.Type,
                CreatedAt = data.CreatedAt
            };
        }

    }
}
