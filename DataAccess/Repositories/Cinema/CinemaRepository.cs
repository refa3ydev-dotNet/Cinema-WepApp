using Core;
using Core.Helpers;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
//using Business.DTOs;
namespace DataAccess.Repositories.CINEMA
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly MoviesDbContext _context;
        public CinemaRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<Cinema> GetCinemaByIdAsync(int id)
        {
            if (id > 0)
            {
                return await _context.Cinemas
                    .Include(x => x.CinemaMovies)
                    .ThenInclude(Task => Task.Movie)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                return null;
            }
        }
        public async Task<List<Cinema>> GetAllCinemasAsync()
        {
            return await _context.Cinemas.ToListAsync();
        }
        public async Task AddCinemaAsync(Cinema cinema)
        {
            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCinemaAsync(Cinema cinema)
        {
            var cine = await _context.Cinemas.FindAsync(cinema.Id);
            if (cine != null)
            {
                if (!string.IsNullOrEmpty(cinema.Logo) && cinema.Logo != cine.Logo)
                {
                    var oldLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Cinemas", Path.GetFileName(cine.Logo));
                    if (File.Exists(oldLogoPath))
                    {
                        File.Delete(oldLogoPath);
                    }
                }
                if (!string.IsNullOrEmpty(cinema.BackgroundPicture) && cinema.BackgroundPicture != cine.BackgroundPicture)
                {
                    var oldBackgroundPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Cinemas", Path.GetFileName(cine.BackgroundPicture));
                    if (File.Exists(oldBackgroundPath))
                    {
                        File.Delete(oldBackgroundPath);
                    }
                }
                if (string.IsNullOrEmpty(cinema.Logo))
                {
                    cinema.Logo = cine.Logo;
                }

                if (string.IsNullOrEmpty(cinema.BackgroundPicture))
                {
                    cinema.BackgroundPicture = cine.BackgroundPicture;
                }

                _context.Entry(cine).CurrentValues.SetValues(cinema);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cinema not found");
            }


        }
        public async Task DeleteCinemaAsync(int id)
        {
            var cine = await _context.Cinemas.FindAsync(id);
            if (cine != null)
            {
                if (!string.IsNullOrEmpty(cine.Logo))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Cinemas", Path.GetFileName(cine.Logo));
                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }
                }
                _context.Cinemas.Remove(cine);

            }
            else
            {
                return;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cinema>> SearchByNameAsync(string name)
        {
            return await _context.Cinemas.Where(x => x.Name.Contains(name)).ToListAsync();
        }
        public async Task<List<Cinema>> SearchByMovieAsync(string name)
        {
            return await _context.Cinemas
                .Include(x => x.CinemaMovies)
                .ThenInclude(z => z.Movie)
                .Where(x => x.CinemaMovies
                .Any(z => z.Movie.Name.Contains(name)))
                .ToListAsync();
        }
        public async Task<int> GetCinemasCountAsync()
        {
            return await _context.Cinemas.CountAsync();
        }


        public async Task<PaginationResult<Cinema>> GetPagedCinemasAsync(int page, int pageSize)
        {
            var totalCount = await _context.Cinemas.CountAsync();
            var TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var cinemas = await _context.Cinemas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginationResult<Cinema>
            {
                Items = cinemas,
                CurrentPage = page,
                TotalPages = TotalPages
            };
        }

        public async Task<List<Cinema>> GetPendingCinemasAsync()
        {
            return await _context.Cinemas
                .Where(x => x.IsApproved == false)
                .ToListAsync();
        }

        public async Task ApproveCinemaAsync(int id)
        {
            var cinema =await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                cinema.IsApproved = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeActivateCinemaAsync(int id)
        {
            var cinema =await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                cinema.IsApproved = false;
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
