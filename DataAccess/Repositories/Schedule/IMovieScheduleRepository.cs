using Core.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Schedule
{
    public interface IMovieScheduleRepository
    {
        Task AddScheduleAsync(MovieSchedule schedule);
    }
}
