using Business.DTOs.Agent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Managers.Agent
{
    public interface IAgentDashboardManager
    {
        Task<AgentDashboardDto> GetAgentDashboardDataAsync(int cinemaId, string agentName);
    }
}
