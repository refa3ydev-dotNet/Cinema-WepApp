using Business.DTOs.Actors;
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
                BackgroundImg = dto.BackgroundUrl,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                PosterImg = dto.PosterUrl,
                Categories = categories.Where(c => dto.CategoryIds.Contains(c.Id)).ToList(),
                Language = dto.Language,
                Translation = dto.Translation,
                ProducerMovies = dto.ProducerIds.Select(ProducerId=>new ProducerMovie
                {
                    ProducerId=ProducerId
                }).ToList()??new List<ProducerMovie>(),
                ActorMovies = dto.ActorsIds?.Select(actorId => new ActorMovie
                {
                    ActorId = actorId
                }).ToList() ?? new List<ActorMovie>(),
                CinemaMovies = dto.CinemasIds?.Select(cinemaId => new CinemaMovie
                {
                    CinemaId = cinemaId
                }).ToList() ?? new List<CinemaMovie>(),
                CreatedDate = DateTime.Now
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
                Categories = categories.Where(c => dto.CategoryIds.Contains(c.Id)).ToList(),
                Language = dto.Language,
                Translation = dto.Translation
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
                CategoryNames = x.Categories.Select(c => c.CategoryName).ToList(),
                Language = x.Language,
                Translation = x.Translation,
                Cinemas = x.CinemaMovies.Select(y => y.Cinema.Name).ToList(),
                Actors = x.ActorMovies.Select(y => y.Actor.FullName).ToList()
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
                ActorsIds = movie.ActorMovies.Select(x => x.ActorId).ToList(),
                CinemasIds = movie.CinemaMovies.Select(x => x.CinemaId).ToList(),
                CategoryIds = movie.Categories.Where(x => x.CategoryName != null).Select(x => x.Id).ToList(),
                CategoryName = movie.Categories.Where(X => X.CategoryName != null).Select(mv => mv.CategoryName).ToList(),
                Language = movie.Language,
                Translation = movie.Translation,
                producer = movie.ProducerMovies.Select(P => new ProducerInMovieDto
                {
                    ID = P.ProducerId,
                    Name = P.Producer.FullName
                }).ToList(),
                actors = movie.ActorMovies.Select(x => new ActorsInMovieDto
                {
                    ID = x.ActorId,
                    Name = x.Actor.FullName
                }).ToList(),

            };
        }


    }
}
