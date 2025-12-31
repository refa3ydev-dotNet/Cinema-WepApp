using Core.Entities;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.DIRECTOR
{
    public class DirectoryRepository : IDirectorRepository
    {
        private readonly MoviesDbContext _context;
        public DirectoryRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task CreateDirectorAsync(Director director)
        {
            await _context.Directors.AddAsync(director);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteDirectorAsync(int id)
        {
            Director director = await GetDirectorByIdAsync(id);
            if (director != null)
            {
                _context.Directors.Remove(director);
            }
            else
            {
                return;
            }
            await _context.SaveChangesAsync();

        }

        public async Task<List<Director>> GetAllDirectorsAsync()
        {
            List<Director> directors = await _context.Directors.ToListAsync();
            if (directors == null)
            {
                return null;
            }
            return directors;
        }

        public async Task<Director> GetDirectorByIdAsync(int id)
        {
            Director director = await _context.Directors.FirstOrDefaultAsync(x => x.Id == id);
            if (director == null)
            {
                return null;
            }
            return director;
        }

        public async Task<Director> GetDirectorByNameAsync(string name)
        {
            Director director = await _context.Directors.FirstOrDefaultAsync(x => x.Name == name);
            if (director == null)
            {
                return null;
            }
            return director;
        }

        public async Task UpdateDirectorAsync(Director director)
        {
            Director existingDirector = await _context.Directors.FindAsync(director.Id);
            if (existingDirector != null)
            {
                if (string.IsNullOrEmpty(director.ProfilePicture))
                {
                    director.ProfilePicture = existingDirector.ProfilePicture;
                }
                _context.Entry(existingDirector).CurrentValues.SetValues(director);
            }
            else
            {
                var dir = await _context.Directors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id == director.Id);
                if (dir != null && string.IsNullOrEmpty(director.ProfilePicture))
                {
                    dir.ProfilePicture = director.ProfilePicture;
                }


            }
            await _context.SaveChangesAsync();
        }
    }
}
