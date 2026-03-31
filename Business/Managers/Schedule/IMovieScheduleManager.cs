using Business.DTOs.Schedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Managers.Schedule
{
    public interface IMovieScheduleManager
    {
        Task CreateScheduleAsync(CreateScheduleDto dto);
        Task<IEnumerable<ScheduleDisplayDto>> GetCinemaSchedulesAsync(int cinemaId);
        Task<bool> DeleteScheduleAsync(int scheduleId, int cinemaId);
         Task<ScheduleDisplayDto> GetScheduleByIdAsync(int scheduleId, int cinemaId);
        Task UpdateScheduleAsync(UpdateScheduleDto dto);
    }
}
