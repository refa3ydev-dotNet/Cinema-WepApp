using Business.DTOs.Movies;
using Business.Managers.Categories;
using Business.Mapping;
using Core.Entities;
using Core.Entities.Relations;
using Core.Helpers;
using DataAccess.Repositories.CINEMA;
using DataAccess.Repositories.MOVIE;
using System.Security.Cryptography.X509Certificates;

namespace Business.Managers.Movies
{
    public class MovieManager : IMovieManager
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ICategoryManager _categoryManager;
        private readonly ICinemaRepository _cinemaRepository;

        public MovieManager(IMovieRepository movieRepository, ICategoryManager categoryManager, ICinemaRepository cinemaRepository)
        {
            _movieRepository = movieRepository;
            _categoryManager = categoryManager;
            _cinemaRepository = cinemaRepository;
        }

        public async Task CreateMovieAsync(CreateMovieDto dto)
        {
            var categories = await _categoryManager.GetCategoriesByIdsAsync(dto.CategoryIds);
            //var categories = categoriesDto.Select(c => new Category
            //{
            //    Id=c.Id,
            //    CategoryName=c.Name
            //}).ToList();
            var movie = dto.ToEntity(categories);
            await _movieRepository.AddMovieAsync(movie);
        }
        public async Task DeleteMovieAsync(int id)
        {
            var exist = await _movieRepository.GetMovieByIdAsync(id);
            if (exist == null) throw new Exception("Movie not found");
            await _movieRepository.DeleteMovieAsync(id);
        }
        public async Task<List<GetAllMoviesDto>> GetAllMoviesAsync()
        {
            var movie = await _movieRepository.GetAllMoviesAsync();
            return movie.ToDto();
        }
        public async Task<GetMovieByIdDto> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            return movie.ToDto();
        }
        public async Task UpdateMovieAsync(UpdateMovieDto dto)
        {
            var categoriesDto = await _categoryManager.GetAllCategoriesAsync();
            var categories = categoriesDto.Select(c => new Category
            {
                Id = c.Id,
                CategoryName = c.Name
            }).ToList();
            var movie = dto.ToEntity(categories);
            await _movieRepository.UpdateMovieAsync(movie);
        }
        public async Task<PaginationResult<GetAllMoviesDto>> GetPagedMoviesAsync(int page, int pageSize)
        {
            var result = await _movieRepository.GetPagedMoviesAsync(page, pageSize);
            var categories = await _categoryManager.GetAllCategoriesAsync();
            var MappedItems = result.Items.ToDto();
            return new PaginationResult<GetAllMoviesDto>
            {
                Items = MappedItems,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages
            };
        }

        public async Task AssignMovieToCinemaAsync(int movieId, int cinemaId)
        {
            var movie=await  _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null) throw new Exception("Movie not found");
            if (!movie.CinemaMovies.Any(cm => cm.CinemaId == cinemaId))
            {
                movie.CinemaMovies.Add(new CinemaMovie { CinemaId = cinemaId, MovieId = movieId });
                await _movieRepository.UpdateMovieAsync(movie);

            }
        }

        public async Task<List<GetAllMoviesDto>> GetMoviesByCinemaIdAsync(int cinemaId)
        {
            var movies = await _movieRepository.GetAllMoviesAsync();
            var cinemaMovies=movies.Where(m=>m.CinemaMovies.Any(cm=>cm.CinemaId==cinemaId)).ToList();
            

            return cinemaMovies.ToDto();
        }

        public async Task<bool> RemoveMovieFromCinemaAsync(int movieId, int cinemaId)
        {
            bool hasAcyiveSchedules=await _movieRepository.HasActiveSchedulesForCinemaAsync(movieId,cinemaId);
            if (hasAcyiveSchedules) return false;
            var movie =await  _movieRepository.GetMovieByIdAsync(movieId);
            if (movie != null)
            {
                var link= movie.CinemaMovies.FirstOrDefault(cm=>cm.CinemaId==cinemaId);
                if (link != null)
                {
                    movie.CinemaMovies.Remove(link);
                    await _movieRepository.UpdateMovieAsync(movie);
                    return true;
                }
            }
            return false;
        }
    }
}
