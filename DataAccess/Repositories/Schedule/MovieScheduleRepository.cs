using Core.Entities.Relations;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

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

        public async Task<MovieSchedule> GetScheduleByIdAndCinemaIdAsync(int scheduleId, int cinemaId)
        {

            return await _context.MovieSchedules
            .Include(ms => ms.Movie)
            .Include(s => s.Room)
            .FirstOrDefaultAsync(ms => ms.Id == scheduleId && ms.CinemaId == cinemaId);
        }

        public async Task<IEnumerable<MovieSchedule>> GetSchedulesByCinemaIdAsync(int cinemaId)
        {
            return await _context.MovieSchedules
            .Include(ms => ms.Movie)
            .Include(s => s.Room)
            .Where(c => c.CinemaId == cinemaId && !c.IsDeleted)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync();
        }

        public async Task UpdateScheduleAsync(MovieSchedule schedule)
        {
            _context.MovieSchedules.Update(schedule);
            await _context.SaveChangesAsync();
        }
    }
}
