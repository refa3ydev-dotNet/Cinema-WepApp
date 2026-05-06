using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Admin
{
    public class DashboardChartsDto
    {

        public List<ChartPointDto> RevenueChart { get; set; } = new();
        public List<ChartPointDto> BookingsChart { get; set; } = new();
    }
}
