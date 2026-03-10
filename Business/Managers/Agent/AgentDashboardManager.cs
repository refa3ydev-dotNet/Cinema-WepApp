using Business.DTOs.Agent;
using Business.Mapping;
using DataAccess.Repositories.CINEMA;
using DataAccess.Repositories.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Managers.Agent
{
    public class AgentDashboardManager : IAgentDashboardManager
    {
        private readonly IAgentDashboardRepository _dashboardRepository;
        private readonly ICinemaRepository _cinemaRepository;
        public AgentDashboardManager(IAgentDashboardRepository dashboardRepository,ICinemaRepository cinemaRepository)
        {
            _dashboardRepository = dashboardRepository;
            _cinemaRepository = cinemaRepository;
        }

        public async Task<AgentDashboardDto> GetAgentDashboardDataAsync(int cinemaId,string agentName)
        {
            var cinema = await _cinemaRepository.GetCinemaByIdAsync(cinemaId);
            var todayBookings = await _dashboardRepository.GetTodayBookingsAsync(cinemaId);
            var activeBookings =await  _dashboardRepository.GetActiveMoviesCountAsync(cinemaId);
            var recentBookings =await  _dashboardRepository.GetRecentBookingAsync(cinemaId,5);

            return AgentDashboardMapping.ToDto(agentName, cinema, todayBookings, activeBookings, recentBookings);
        }


    }
}
