using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Dashboard
{
    public interface IAgentDashboardRepository
    {
        Task<List<Booking>>GetTodayBookingsAsync(int cinemaId);
        Task<int> GetActiveMoviesCountAsync(int cinemaId);
        Task<List<Booking>> GetRecentBookingAsync(int cinemaId,int count);
    }
}
