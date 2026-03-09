using Business.DTOs.Cinemas;
using Business.DTOs.Movies;
using Core;

namespace Business.Mapping
{
    public static class CinemaMapping
    {
        public static Cinema ToEntity(this CreateCinemaDto dto)
        {
            return new Cinema()
            {
                Name = dto.Name,
                Logo = dto.LogoPath,
                Description = dto.Description,
                Address = dto.Address,
                BackgroundPicture = dto.BackgroundPath
            };
        }

        public static Cinema ToEntity(this UpdateCinemaDto dto)
        {
            return new Cinema()
            {
                Id = dto.Id,
                Name = dto.Name,
                Logo = dto.LogoPath,
                Description = dto.Description,
                Address = dto.Address,
                BackgroundPicture = dto.BackgroundPath
            };
        }
        public static Cinema ToEntity(this FixCinemaApplicationDto dto)
        {
            return new Cinema()
            {
                Id = dto.Id,
                Name = dto.Name,
                Logo = dto.LogoPath,
                Description = dto.Description,
                Address = dto.Address,
                BackgroundPicture = dto.BackgroundPicturePath,
                UpdatedAt = DateTime.Now,
                
                
            };
        }

        public static List<GetAllCinemasDto> ToDto(this List<Cinema> cinemas)
        {
            if (cinemas == null) return null;
            return cinemas.Select(cinema => new GetAllCinemasDto()
            {
                Id = cinema.Id,
                Name = cinema.Name,
                LogoPath = cinema.Logo,
                Description = cinema.Description,
                Address = cinema.Address,
                BackgroundPath = cinema.BackgroundPicture,
                AgentName= ""
            }).ToList();
        }

        public static GetCinemaByIdDto ToDto(this Cinema cinema)
        {
            if (cinema == null) return null;
            return new GetCinemaByIdDto()
            {
                Id = cinema.Id,
                Name = cinema.Name,
                LogoPath = cinema.Logo,
                Description = cinema.Description,
                Address = cinema.Address,
                BackgroundPath = cinema.BackgroundPicture,
                MovieDetails = cinema.CinemaMovies?.Where(x => x.Movie != null)
                .Select(movie => new GetAllMoviesDto
                {
                    Id = movie.Movie.Id,
                    Name = movie.Movie.Name,
                    PosterUrl = movie.Movie.PosterImg
                }).ToList(),
                
            };
        }

    }
}
