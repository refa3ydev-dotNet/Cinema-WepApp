using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Admin
{
    public class DashboardChartsResult
    {

        public List<ChartPointResult> RevenueChart { get; set; } = new();
        public List<ChartPointResult> BookingsChart { get; set; } = new();
    }
}
