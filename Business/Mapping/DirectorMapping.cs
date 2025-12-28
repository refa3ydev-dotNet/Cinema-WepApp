using Business.DTOs.Directors;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                BirthDate = dto.BirthDate
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
                BirthDate = dto.BirthDate   
            };
        }

        public static List<GetAllDirectorDto> ToDto(this List<Director> entity)
        {
            return entity.Select(director=> new GetAllDirectorDto()
            {
                Name = director.Name,
                Biography = director.Biography,
                ProfilePictureUrl = director.ProfilePicture,
                IMDB = director.IMDB,
                BirthDate = director.BirthDate
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
                BirthDate = entity.BirthDate
            };
        }
    }
}
