using Business.DTOs.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers.Actors
{
    public interface IActorsManager
    {
        Task CreateActorAsync(CreateActorDto dto);
        Task DeleteActorAsync(int id);
        Task<List<GetAllActorsDto>> GetAllActorsAsync();
        Task<GetActorByIdDto> GetActorByIdAsync(int id);
        Task UpdateActorAsync(UpdateActorDto dto);

    }
}
