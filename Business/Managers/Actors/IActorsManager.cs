using Business.DTOs.Actors;

namespace Business.Managers.Actors
{
    public interface IActorsManager
    {
        Task CreateActorAsync(CreateActorDto dto);
        Task DeleteActorAsync(int id);
        Task<List<GetAllActorsDto>> GetAllActorsAsync();
        Task<GetActorByIdDto> GetActorByIdAsync(int id);
        Task UpdateActorAsync(UpdateActorDto dto);
        Task<List<GetAllActorsDto>> SearchActorsAsync(string name);

    }
}
