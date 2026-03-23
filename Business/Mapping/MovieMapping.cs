using Business.DTOs.Actors;
using Business.DTOs.Cinemas;
using Business.DTOs.Directors;
using Business.DTOs.Movies;
using Business.DTOs.Producers;
using Core;
using Core.Entities;
using Core.Entities.Relations;


namespace Business.Mapping
{
    public static class MovieMapping
    {
        public static Movie ToEntity(this CreateMovieDto dto, List<Category> categories)
        {
            return new Movie()
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                PosterImg = dto.PosterUrl,
                BackgroundImg = dto.BackgroundUrl,
                Language = dto.Language,
                Translation = dto.Translation,

                Categories = dto.CategoryIds!=null
                ?categories.Where(c => dto.CategoryIds.Contains(c.Id)).ToList()
                :new List<Category>(),
                ProducerMovies = dto.ProducerIds.Select(Id=>new ProducerMovie
                {
                    ProducerId=Id
                }).ToList()??new List<ProducerMovie>(),
                ActorMovies = dto.ActorsIds?.Select(Id => new ActorMovie
                {
                    ActorId = Id
                }).ToList() ?? new List<ActorMovie>(),
                DirectorMovies=dto.DirectorIds?.Select(Id => new DirectorMovie
                {
                    DirectorId = Id
                }).ToList() ?? new List<DirectorMovie>(),
                
            };
        }

        public static Movie ToEntity(this UpdateMovieDto dto, List<Category> categories)
        {
            return new Movie()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                PosterImg = dto.PosterUrl,
                BackgroundImg = dto.BackgroundUrl,
                Language = dto.Language,
                Translation = dto.Translation,
                UpdatedAt = DateTime.Now,
                Categories = dto.CategoryIds != null
                ? categories.Where(c => dto.CategoryIds.Contains(c.Id)).ToList()
                : new List<Category>(),
                ProducerMovies = dto.ProducerIds.Select(Id => new ProducerMovie
                {
                    ProducerId = Id
                }).ToList() ?? new List<ProducerMovie>(),
                ActorMovies = dto.ActorsIds?.Select(Id => new ActorMovie
                {
                    ActorId = Id
                }).ToList() ?? new List<ActorMovie>(),
                DirectorMovies = dto.DirectorIds?.Select(Id => new DirectorMovie
                {
                    DirectorId = Id
                }).ToList() ?? new List<DirectorMovie>(),
            };
        }

        public static List<GetAllMoviesDto> ToDto(this List<Movie> movies)
        {
            if (movies == null) return null;
            return movies.Select(x => new GetAllMoviesDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                PosterUrl = x.PosterImg,
                BackgroundUrl = x.BackgroundImg,
                Language = x.Language,
                Translation = x.Translation,
                CategoryNames = x.Categories.Select(c => c.CategoryName).ToList(),
                Cinemas = x.CinemaMovies?.Select(y => y.Cinema.Name).ToList()?? new List<string>(),
                Actors = x.ActorMovies?.Select(y => y.Actor.FullName).ToList()?? new List<string>(),
                Directors = x.DirectorMovies?.Select(y => y.Director.Name).ToList()?? new List<string>(),
                Producers = x.ProducerMovies?.Select(y => y.Producer.FullName).ToList()??new List<string>(),
            }).ToList();
        }

        public static GetMovieByIdDto ToDto(this Movie movie)
        {
            if (movie == null) return null;
            return new GetMovieByIdDto()
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                PosterUrl = movie.PosterImg,
                BackgroundUrl = movie.BackgroundImg,
                Language = movie.Language,
                Translation = movie.Translation,
                ReleaseDate = movie.CreatedAt,
                CategoryIds = movie.Categories.Select(x => x.Id).ToList(),
                ActorsIds = movie.ActorMovies?.Select(x => x.ActorId).ToList()??new List<int>(),
                CinemasIds = movie.CinemaMovies?.Select(x => x.CinemaId).ToList()??new List<int>(),
                DirectorsIds = movie.DirectorMovies?.Select(x => x.DirectorId).ToList()??new List<int>(),
                ProducersIds = movie.ProducerMovies?.Select(x => x.ProducerId).ToList()??new List<int>(),

                CategoryName = movie.Categories?.Select(mv => mv.CategoryName).ToList()??new List<string>(),
                Producers = movie.ProducerMovies?.Select(P => new ProducerInMovieDto
                {
                    ID = P.ProducerId,
                    Name = P.Producer.FullName,
                    Image = P.Producer.ProfilePicture
                }).ToList()??new List<ProducerInMovieDto>(),
                Actors = movie.ActorMovies?.Select(x => new ActorsInMovieDto
                {
                    ID = x.ActorId,
                    Name = x.Actor.FullName,
                    Image = x.Actor.ProfilePicture
                }).ToList()??new List<ActorsInMovieDto>(),
                Cinemas = movie.CinemaMovies?.Select(x => new CinemaMoviesDto
                {
                    ID = x.CinemaId,
                    Name = x.Cinema.Name,
                     Image = x.Cinema.Logo
                }).ToList()??new List<CinemaMoviesDto>(),
                Directors = movie.DirectorMovies?.Select(x => new DirectorMoviesDto
                {
                    ID = x.DirectorId,
                    Name = x.Director.Name,
                    Image = x.Director.ProfilePicture
                }).ToList()??new List<DirectorMoviesDto>(),

            };
        }


    }
}
