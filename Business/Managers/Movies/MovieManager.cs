using Business.DTOs.Movies;
using Business.Managers.Categories;
using Business.Mapping;
using Core.Entities;
using Core.Helpers;
using DataAccess.Repositories.MOVIE;

namespace Business.Managers.Movies
{
    public class MovieManager: IMovieManager
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ICategoryManager _categoryManager;

        public MovieManager(IMovieRepository movieRepository, ICategoryManager categoryManager)
        {
            _movieRepository = movieRepository;
            _categoryManager = categoryManager;
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
            var MappedItems = result.Items.Select(c => new GetAllMoviesDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Price = c.Price,
                PosterUrl = c.PosterImg,
                CategoryNames = c.Categories.Select(cm => cm.CategoryName).Where(name => name != null).ToList() ?? new List<string>(),
                Language = c.Language,
                Translation = c.Translation,
                Cinemas = c.CinemaMovies.Select(cm => cm.Cinema.Name).Where(name => name != null).ToList() ?? new List<string>(),
                Actors = c.ActorMovies.Select(am => am.Actor.FullName).Where(name => name != null).ToList() ?? new List<string>()

            }).ToList();
            return new PaginationResult<GetAllMoviesDto>
            {
                Items = MappedItems,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages
            };
        }
    }
}
