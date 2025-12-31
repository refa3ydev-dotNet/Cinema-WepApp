using Core;
using Core.Helpers;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories.MOVIE
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesDbContext _context;

        public MovieRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            if (id > 0)
            {
                var mov = await _context.Movies
                    .Include(x => x.ActorMovies)
                    .ThenInclude(y => y.Actor)
                    .Include(x => x.CinemaMovies)
                    .ThenInclude(y => y.Cinema)
                    .Include(x => x.Categories)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (mov != null)
                {

                    return mov;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            var movs = await _context.Movies.Include(x => x.ActorMovies)
                .ThenInclude(y => y.Actor)
                .Include(x => x.CinemaMovies)
                .ThenInclude(y => y.Cinema)
                .ToListAsync();
            if (movs != null)
            {

                return movs;
            }
            else
            {
                return null;
            }
        }
        public async Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            var mov = await _context.Movies.FindAsync(movie.Id);
            if (mov != null)
            {
                if (!string.IsNullOrEmpty(mov.PosterImg) && movie.PosterImg != mov.PosterImg)
                {
                    var OldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Movies", Path.GetFileName(mov.PosterImg));
                    if (File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                    }
                }
                _context.Entry(mov).CurrentValues.SetValues(movie);
            }
            else
            {
                var dbmov = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == movie.Id);
                if (dbmov != null && string.IsNullOrEmpty(movie.PosterImg))
                {
                    movie.PosterImg = dbmov.PosterImg;
                }

            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMovieAsync(int id)
        {
            var mov = await _context.Movies.FindAsync(id);
            if (mov != null)
            {
                _context.Movies.Remove(mov);

            }
            else
            {
                return;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Movie>> SearchByNameAsync(string name)
        {
            return await _context.Movies.Where(x => x.Name.Contains(name)).ToListAsync();
        }
        public async Task<int> GetMoviesCountAsync()
        {
            return await _context.Movies.CountAsync();
        }
        public async Task<PaginationResult<Movie>> GetPagedMoviesAsync(int page, int pageSize)
        {
            var Query = _context.Movies.Include(m => m.CinemaMovies)
                .ThenInclude(c => c.Cinema)
                .Include(x => x.ActorMovies)
                .ThenInclude(y => y.Actor);
            var totalCount = await _context.Movies.CountAsync();
            var TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var movies = await Query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginationResult<Movie>
            {
                Items = movies,
                CurrentPage = page,
                TotalPages = TotalPages
            };
        }
        public async Task<List<Movie>> GetMovieByActorAsync(string name)
        {
            return await _context.Movies
                .Include(x => x.ActorMovies)
                .ThenInclude(y => y.Actor)
                .Where(c => c.ActorMovies
                .Any(z => z.Actor.FullName.Contains(name)))
                .ToListAsync();
        }
    }
}
