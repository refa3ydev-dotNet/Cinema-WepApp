using Business.DTOs.Directors;
using Core.Entities;

namespace Business.Mapping
{
    public static class DirectorMapping
    {

        public static Director ToEntity(this CreateDirectorDto dto)
        {
            if (dto == null) return null;
            return new Director()
            {
                Name = dto.Name,
                Biography = dto.Biography,
                ProfilePicture = dto.ProfilePictureUrl,
                IMDB = dto.IMDB,
                BirthDate = dto.BirthDate,
                DeathDate = dto.DeathDate,
                Nationality = dto.Nationality,
                

            };
        }

        public static Director ToEntity(this UpdateDirectorDto dto)
        {
            return new Director()
            {
                Name = dto.Name,
                Biography = dto.Biography,
                ProfilePicture = dto.ProfilePictureUrl,
                IMDB = dto.IMDB,
                BirthDate = dto.BirthDate,
                DeathDate = dto.DeathDate,
                Nationality = dto.Nationality
            };
        }

        public static List<GetAllDirectorDto> ToDto(this List<Director> entity)
        {
            if (entity == null) return null;
            return entity.Select(director => new GetAllDirectorDto
            {
                Name = director.Name,
                Biography = director.Biography,
                ProfilePictureUrl = director.ProfilePicture,
                IMDB = director.IMDB,
                BirthDate = director.BirthDate,
                DeathDate = director.DeathDate,
                Nationality = director.Nationality
            }).ToList();
        }
        public static GetDirectorByIdDto ToDto(this Director entity)
        {
            return new GetDirectorByIdDto()
            {
                Name = entity.Name,
                Biography = entity.Biography,
                ProfilePictureUrl = entity.ProfilePicture,
                IMDB = entity.IMDB,
                BirthDate = entity.BirthDate,
                DeathDate = entity.DeathDate,
                Nationality = entity.Nationality
            };
        }
    }
}
