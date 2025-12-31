using Business.DTOs.Actors;
using Business.DTOs.Movies;
using Core;

namespace Business.Mapping
{
    public static class ActorMapping
    {
        public static Actor ToActor(this CreateActorDto dto)
        {
            return new Actor()
            {
                FullName = dto.FullName,
                Bio = dto.Bio,
                ProfilePicture = dto.ProfilePath,
                IMDBLink = dto.IMDBLink,
                BirthDate = dto.BirthDate
            };
        }
        public static Actor ToActor(this UpdateActorDto dto)
        {
            return new Actor()
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Bio = dto.Bio,
                ProfilePicture = dto.ProfilePath,
                IMDBLink = dto.IMDBLink,
                BirthDate = dto.BirthDate
            };

        }
        public static GetActorByIdDto ToActor(this Actor actor)
        {
            if (actor == null) return null;
            return new GetActorByIdDto()
            {
                FullName = actor.FullName,
                Bio = actor.Bio,
                ProfilePath = actor.ProfilePicture,
                IMDBLink = actor.IMDBLink,
                BirthDate =(DateOnly)actor.BirthDate
                

            };
        }
        public static GetActorByIdDto ToActorWithMovies(this Actor actor)
        {
            if (actor == null) return null;

            return new GetActorByIdDto
            {
                Id = actor.Id,
                FullName = actor.FullName,
                Bio = actor.Bio,
                ProfilePath = actor.ProfilePicture,
                IMDBLink = actor.IMDBLink,
                MovieDetails = actor.ActorMovies?
                .Select(m => new GetAllMoviesDto
                {
                    Id = m.Movie.Id,
                    Name = m.Movie.Name,
                    PosterUrl = m.Movie.PosterImg,
                    Price = m.Movie.Price,
                    Cinemas = m.Movie.CinemaMovies.Select(cm => cm.Cinema.Name).Where(name => name != null).ToList() ?? new List<string>(),
                }).ToList(),
                BirthDate = (DateOnly)actor.BirthDate
            };
        }
        public static List<GetAllActorsDto> ToActor(this List<Actor> actors)
        {
            if (actors == null) return null;
            return actors.Select(actor => new GetAllActorsDto
            {
                Id = actor.Id,
                FullName = actor.FullName,
                Bio = actor.Bio,
                ProfilePath = actor.ProfilePicture,
                IMDBLink = actor.IMDBLink,
            }).ToList();
        }


    }
}
