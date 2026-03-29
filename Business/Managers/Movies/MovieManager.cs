using Business.DTOs.Integration;
using Business.DTOs.Movies;
using Business.Managers.Categories;
using Business.Mapping;
using Business.Services.TmdbService;
using Core;
using Core.Entities;
using Core.Entities.Relations;
using Core.Enums;
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
        public readonly ITmdbService _tmdbService;

        public MovieManager(IMovieRepository movieRepository, ICategoryManager categoryManager,
            ICinemaRepository cinemaRepository, ITmdbService tmdbService)
        {
            _movieRepository = movieRepository;
            _categoryManager = categoryManager;
            _cinemaRepository = cinemaRepository;
            _tmdbService = tmdbService;
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
        public async Task SyncMovieFromTmdbAsync(int tmdbMovieId, int cinemaId)
        {
            var tmdbDetails= await _tmdbService.GetMovieDetailsAsync(tmdbMovieId);
            if (tmdbDetails == null)
            {
                throw new Exception("Movie not found in TMDB");
            }
            var trailerKey = tmdbDetails.Videos?.Results?.FirstOrDefault(v => v.Type == "Trailer"&&v.Site=="YouTube")?.Key;
            var actorMoves = new List<ActorMovie>();
            var topCast= tmdbDetails.Credits?.Cast?.Take(6)??Enumerable.Empty<TmdbCast>();
            foreach (var cast in topCast)
            {
                var persomInfo=await _tmdbService.GetPersonDetailsAsync(cast.Id);
                actorMoves.Add(cast.ToActorMovie(persomInfo));
            }
            var directorMovies=new List<DirectorMovie>();
            var directors=tmdbDetails.Credits?.Crew?.Where(c=>c.Job=="Director")??Enumerable.Empty<TmdbCrew>();
            foreach (var director in directors)
            {
                var persomInfo=await _tmdbService.GetPersonDetailsAsync(director.Id);
                directorMovies.Add(director.ToDirectorMovie(persomInfo));
            }
            var producerMovies=new List<ProducerMovie>();
            var producers=tmdbDetails.Credits?.Crew?.Where(c=>c.Job=="Producer")??Enumerable.Empty<TmdbCrew>();
            foreach (var producer in producers)
            {
                var persomInfo=await _tmdbService.GetPersonDetailsAsync(producer.Id);
                producerMovies.Add(producer.ToProducerMovie(persomInfo));
            }
            var movieToSave = new Movie
            {
                TmdbId = tmdbDetails.Id,
                Name = tmdbDetails.Title,
                Description = tmdbDetails.Overview,
                Language = MapTmdbLanguage(tmdbDetails.Original_Language) ,
                Price = 100,
                Translation = DetermineTranslation(tmdbDetails.Original_Language),
                PosterImg = !string.IsNullOrEmpty(tmdbDetails.Poster_Path) ? $"https://image.tmdb.org/t/p/w500{tmdbDetails.Poster_Path}" : string.Empty,
                BackgroundImg = !string.IsNullOrEmpty(tmdbDetails.Backdrop_Path) ? $"https://image.tmdb.org/t/p/original{tmdbDetails.Backdrop_Path}" : string.Empty,
                TrailerUrl = trailerKey != null ? $"https://www.youtube.com/embed/{trailerKey}" : string.Empty,
                Rating = (decimal)tmdbDetails.Vote_Average,
                ReleaseDate = tmdbDetails.Release_Date,
                Runtime = tmdbDetails.Runtime,
                

                // التصنيفات
                Categories = tmdbDetails.Genres?.Select(g => new Category {
                    CategoryName = g.Name,
                    Description=$"Movies belonging to the {g.Name} genre.",
                    ImageUrl = "/Images/NotFound/logo.jpg"
                }).ToList() ?? new List<Category>(),
                CinemaMovies = new List<CinemaMovie>
        {
            new CinemaMovie { CinemaId = cinemaId } // الـ MovieId هياخده أوتوماتيك لما الـ Movie يتحفظ
        }
            };
            movieToSave.ActorMovies = actorMoves;
            movieToSave.DirectorMovies = directorMovies;
            movieToSave.ProducerMovies = producerMovies;
            var SavedMovie=await _movieRepository.UpsertMovieFromTmdbAsync(movieToSave);
            
            if (SavedMovie.CinemaMovies == null)
            {
                SavedMovie.CinemaMovies=new List<CinemaMovie>();
            }
            if (!SavedMovie.CinemaMovies.Any(cm => cm.CinemaId == cinemaId))
            {
                SavedMovie.CinemaMovies.Add(new CinemaMovie { CinemaId = cinemaId, MovieId = SavedMovie.Id });
                await _movieRepository.UpdateMovieAsync(SavedMovie);
            }

        }
        private Language MapTmdbLanguage(string tmdbLangCode)
        {
        if (string.IsNullOrEmpty(tmdbLangCode)) return Language.English; // قيمة افتراضية

        return tmdbLangCode.ToLower() switch
        {
            "en" => Language.English,
            "ar" => Language.Arabic,
            "es" => Language.Spanish,
            "fr" => Language.French,
            "de" => Language.German,
            "it" => Language.Italian,
            "ja" => Language.Japanese,
            "zh" => Language.Chinese, 
            "ru" => Language.Russian,
            "pt" => Language.Portuguese,
            "tr" => Language.Turkish,
            "hi" => Language.Hindi,
            "ko" => Language.Korean,
            "ur" => Language.Urdu,
            "fa" => Language.Persian, 
            _ => Language.English 
        };
    }


    private TranslationType DetermineTranslation(string tmdbLangCode)
    {
        if (string.IsNullOrEmpty(tmdbLangCode)) return TranslationType.Subtitled;

        return tmdbLangCode.ToLower() == "ar" ? TranslationType.None : TranslationType.Subtitled;
    }
}
}
