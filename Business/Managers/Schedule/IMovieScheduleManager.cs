using Business.DTOs.Schedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Managers.Schedule
{
    public interface IMovieScheduleManager
    {
        Task CreateScheduleAsync(CreateScheduleDto dto);
    }
}
