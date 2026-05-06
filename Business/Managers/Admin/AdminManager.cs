using Business.DTOs.Admin;
using Business.Mapping;
using DataAccess.Repositories.Admin;

namespace Business.Managers.Admin
{
    public class AdminManager : IAdminManager
    {
        private readonly IAdminDashboardRepository _adminRepository;
        public AdminManager(IAdminDashboardRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<AdminDashboardDto> GetDashboardAsync(int days = 7)
        {
            days = NormalizeDays(days);

            var summary = await _adminRepository.GetSummaryAsync();
            var revenueChart = await _adminRepository.GetRevenueChartAsync(days);
            var bookingsChart = await _adminRepository.GetBookingsChartAsync(days);
            var pendingApprovals = await _adminRepository.GetPendingCinemasAsync(5);
            var topMovies = await _adminRepository.GetTopMoviesAsync(5);
            var recentActivities = await _adminRepository.GetRecentActivitiesAsync(6);

            return new AdminDashboardDto
            {
                Summary = summary.ToDto(),
                RevenueChart = revenueChart.Select(x => x.ToDto()).ToList(),
                BookingsChart = bookingsChart.Select(x => x.ToDto()).ToList(),
                PendingCinemas = pendingApprovals.Select(x => x.ToDto()).ToList(),
                TopMovies = topMovies.Select(x => x.ToDto()).ToList(),
                RecentActivities = recentActivities.Select(x => x.ToDto()).ToList()
            };
        }

        public async Task<DashboardChartsDto> GetChartsAsync(int days = 7)
        {
            days = NormalizeDays(days);

            var revenueChart = await _adminRepository.GetRevenueChartAsync(days);
            var bookingsChart = await _adminRepository.GetBookingsChartAsync(days);

            return new DashboardChartsDto
            {
                RevenueChart = revenueChart.Select(x => x.ToDto()).ToList(),
                BookingsChart = bookingsChart.Select(x => x.ToDto()).ToList()
            };
        }

        private static int NormalizeDays(int days)
        {
            return days switch
            {
                7 => 7,
                30 => 30,
                90 => 90,
                _ => 7
            };
        }
    }
}
