using System;
using System.Collections.Generic;
using Business.DTOs.Admin;

namespace DataAccess.Repositories.Admin
{
    public interface IAdminDashboardRepository
    {
        Task<DashboardSummaryResult> GetSummaryAsync();
        Task<List<ChartPointResult>> GetRevenueChartAsync(int days);
        Task<List<ChartPointResult>> GetBookingsChartAsync(int days);

        Task<List<PendingCinemaApprovalResult>> GetPendingCinemasAsync(int take=5);
        Task<List<TopMoviesDashboardResult>> GetTopMoviesAsync(int take=5);
        Task<List<RecentActivityResult>> GetRecentActivitiesAsync(int take=6);
    }
}
