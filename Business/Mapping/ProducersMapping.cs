using Business.DTOs.Movies;
using Business.DTOs.Producers;
using Core;

namespace Business.Mapping
{
    public static class ProducersMapping
    {
        public static Producer ToEntity(this CreateProducerDto dto)
        {
            return new Producer()
            {
                FullName = dto.FullName,
                Bio = dto.Bio,
                ProfilePicture = dto.ProfilePath,
                IMDB = dto.IMDBLink,
                BirthDate = dto.BirthDate,
                DeathDate = dto.DeathDate,
                Nationality = dto.Nationality,
                
            };
        }
        public static Producer ToEntity(this UpdateProducerDto dto)
        {
            return new Producer()
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Bio = dto.Bio,
                ProfilePicture = dto.ProfilePath,
                IMDB = dto.IMDBLink,
                BirthDate = dto.BirthDate,
                DeathDate = dto.DeathDate,
                Nationality = dto.Nationality
            };

        }
        public static GetProducerByIdDto ToDto(this Producer Producer)
        {
            if (Producer == null) return null;
            return new GetProducerByIdDto()
            {
                FullName = Producer.FullName,
                Bio = Producer.Bio,
                ProfilePath = Producer.ProfilePicture,
                IMDBLink = Producer.IMDB,
                BirthDate = Producer.BirthDate,
                DeathDate = Producer.DeathDate,
                Nationality = Producer.Nationality

            };
        }
        public static GetProducerByIdDto ToProducerWithMovies(this Producer Producer)
        {
            if (Producer == null) return null;

            return new GetProducerByIdDto
            {
                Id = Producer.Id,
                FullName = Producer.FullName,
                Bio = Producer.Bio,
                ProfilePath = Producer.ProfilePicture,
                IMDBLink = Producer.IMDB,
                BirthDate = Producer.BirthDate,
                DeathDate = Producer.DeathDate,
                Nationality = Producer.Nationality,
                MovieDetails = Producer.ProducerMovies                
                .Select(m => m.Movie)
                .Where(m => m != null)
                .Select(m=> new GetAllMoviesDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    PosterUrl = m.PosterImg,
                    Price = m.Price,
                    Cinemas = m.CinemaMovies.Select(cm => cm.Cinema.Name).Where(name => name != null).ToList() ?? new List<string>(),

                }).ToList()
            };
        }
        public static List<GetAllProducersDto> ToDto(this List<Producer> Producer)
        {
            if (Producer == null) return null;
            return Producer.Select(Producer => new GetAllProducersDto
            {
                Id = Producer.Id,
                FullName = Producer.FullName,
                Bio = Producer.Bio,
                ProfilePath = Producer.ProfilePicture,
                IMDBLink = Producer.IMDB,
                BirthDate = Producer.BirthDate,
                DeathDate = Producer.DeathDate,
                Nationality = Producer.Nationality

            }).ToList();
        }


    }
}
