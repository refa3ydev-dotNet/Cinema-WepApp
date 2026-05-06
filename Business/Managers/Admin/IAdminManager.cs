using Business.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Managers.Admin
{
    public interface IAdminManager
    {
        Task<AdminDashboardDto> GetDashboardAsync(int days = 7);
        Task<DashboardChartsDto> GetChartsAsync(int days = 7);
    }
}
