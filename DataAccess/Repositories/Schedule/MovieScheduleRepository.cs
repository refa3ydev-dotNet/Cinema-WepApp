using Core.Entities.Relations;
using DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories.Schedule
{
    public class MovieScheduleRepository : IMovieScheduleRepository
    {
        private readonly MoviesDbContext _context;
        public MovieScheduleRepository(MoviesDbContext context)
        {
            _context = context;
        }
        public async Task AddScheduleAsync(MovieSchedule schedule)
        {
            await _context.MovieSchedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }
    }
}
