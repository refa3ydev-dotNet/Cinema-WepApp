using Core.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Schedule
{
    public interface IMovieScheduleRepository
    {
        Task AddScheduleAsync(MovieSchedule schedule);
        Task<IEnumerable<MovieSchedule>> GetSchedulesByCinemaIdAsync(int cinemaId);
        Task<MovieSchedule> GetScheduleByIdAndCinemaIdAsync(int scheduleId, int cinemaId);
        Task UpdateScheduleAsync(MovieSchedule schedule);
        Task<MovieSchedule> GetScheduleWithDetailsByIdAsync(int scheduleId);
        Task<IEnumerable<MovieSchedule>> GetActiveSchedulesByMovieIdAsync(int movieId);
    }
}
