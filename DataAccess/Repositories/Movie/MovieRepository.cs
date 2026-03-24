using Core;
using Core.Entities;
using Core.Entities.Relations;
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
                var mov = await _context.Movies
                    .Include(x => x.ActorMovies)
                    .ThenInclude(y => y.Actor)
                    .Include(x => x.CinemaMovies)
                    .ThenInclude(y => y.Cinema)
                    .Include(p=>p.ProducerMovies)
                    .ThenInclude(y=>y.Producer)
                    .Include(d=>d.DirectorMovies)
                    .ThenInclude(y=>y.Director)
                    .Include(x => x.Categories)
                    .FirstOrDefaultAsync(x => x.Id == id);

                return mov ?? new Movie();
        }
        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            var movs = await _context.Movies
                .Include(x => x.ActorMovies).ThenInclude(y => y.Actor)
                .Include(x => x.CinemaMovies).ThenInclude(y => y.Cinema)
                .Include(p => p.ProducerMovies).ThenInclude(y => y.Producer)
                .Include(d => d.DirectorMovies).ThenInclude(y => y.Director)
                .Include(x => x.Categories)
                .ToListAsync();
                return movs ?? new List<Movie>();
        }
        public async Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            var dbmovie = await _context.Movies
                .Include(x => x.ActorMovies)
                .Include(x => x.CinemaMovies)
                .Include(x => x.Categories)
                .Include(x => x.ProducerMovies)
                .Include(x => x.DirectorMovies)
                .FirstOrDefaultAsync(x => x.Id == movie.Id);

            if (dbmovie != null)
            {
                _context.Entry(dbmovie).CurrentValues.SetValues(movie);
                dbmovie.Categories.Clear();
                foreach (var category in movie.Categories)
                {
                    dbmovie.Categories.Add(category);
                }

                _context.ActorMovies.RemoveRange(dbmovie.ActorMovies);
                dbmovie.ActorMovies = movie.ActorMovies;

                _context.CinemaMovies.RemoveRange(dbmovie.CinemaMovies);
                dbmovie.CinemaMovies = movie.CinemaMovies;

                _context.ProducerMovies.RemoveRange(dbmovie.ProducerMovies);
                dbmovie.ProducerMovies = movie.ProducerMovies;

                _context.DirectorMovies.RemoveRange(dbmovie.DirectorMovies);
                dbmovie.DirectorMovies = movie.DirectorMovies;

                await _context.SaveChangesAsync();
            }
            //else
            //{
            //    var dbmov = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == movie.Id);
            //    if (dbmov != null && string.IsNullOrEmpty(movie.PosterImg))
            //    {
            //        movie.PosterImg = dbmov.PosterImg;
            //    }

            //}
        }
        public async Task DeleteMovieAsync(int id)
        {
            var mov = await _context.Movies.FindAsync(id);
            if (mov != null)
            {
                string[] imageToDelete = {mov.PosterImg,mov.BackgroundImg};
                foreach (var image in imageToDelete)
                {
                    if (!string.IsNullOrEmpty(image) && !image.StartsWith("http"))
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.TrimStart('/'));

                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }
                }
                _context.Movies.Remove(mov);
            await _context.SaveChangesAsync();

            }
            else
            {
                return;
            }
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
            var Query = _context.Movies
                .Include(m => m.CinemaMovies).ThenInclude(c => c.Cinema)
                .Include(x => x.ActorMovies).ThenInclude(y => y.Actor)
                .Include(p=>p.ProducerMovies).ThenInclude(y=>y.Producer)
                .Include(d=>d.DirectorMovies).ThenInclude(y => y.Director)
                .Include(x => x.Categories);
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

        public async Task<bool> HasActiveSchedulesForCinemaAsync(int movieId, int cinemaId)
        {
            return await _context.MovieSchedules
                .Include(x => x.Cinema)
                .AnyAsync(s=>s.MovieId==movieId && s.CinemaId==cinemaId && s.StartDate>DateTime.Now);
        }

        public async Task<Movie> UpsertMovieFromTmdbAsync(Movie movie)
        {
            var existingMovie = await _context.Movies.Include(m=>m.CinemaMovies)
                .FirstOrDefaultAsync(m => m.TmdbId == movie.TmdbId);
            if(existingMovie != null)
            {
                return existingMovie;
            }
            var newCategories = new List<Category>();
            foreach (var category in movie.Categories)
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == category.CategoryName);
                if (existingCategory != null)
                {
                    newCategories.Add(existingCategory);
                }
                else
                {
                    newCategories.Add(category);
                }
            }
            movie.Categories = newCategories;

            var newActorMovies = new List<ActorMovie>();
            foreach (var actor in movie.ActorMovies)
            {
                var existingActor = await _context.Actors.FirstOrDefaultAsync(a => a.FullName == actor.Actor.FullName);
                if (existingActor != null)
                {
                    newActorMovies.Add(new ActorMovie { ActorId = existingActor.Id });
                }
                else
                {
                    newActorMovies.Add(actor);
                }
            }
            movie.ActorMovies = newActorMovies;
            
            var newDirectorMovies = new List<DirectorMovie>();
            foreach (var director in movie.DirectorMovies)
            {

                var existingDirector = await _context.Directors.FirstOrDefaultAsync(d => d.Name == director.Director.Name);
                if (existingDirector != null)
                {
                    newDirectorMovies.Add(new DirectorMovie { DirectorId = existingDirector.Id });
                }
                else
                {
                    newDirectorMovies.Add(director);
                }
            }
            movie.DirectorMovies = newDirectorMovies;

            var newProducerMovies = new List<ProducerMovie>();
            foreach (var producer in movie.ProducerMovies)
            {
                var existingProducer = await _context.Producers.FirstOrDefaultAsync(p => p.FullName == producer.Producer.FullName);
                if (existingProducer != null)
                {
                    newProducerMovies.Add(new ProducerMovie { ProducerId = existingProducer.Id });
                }
                else
                {
                    newProducerMovies.Add(producer);
                }
            }
            movie.ProducerMovies = newProducerMovies;

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return movie;
            }
        }
    }

