using Business.DTOs.Actors;
using Business.Mapping;
using DataAccess.Repositories.ACTOR;

namespace Business.Managers.Actors
{
    public class ActorsManager : IActorsManager
    {
        private readonly IActorRepository _actorRepository;
        public ActorsManager(IActorRepository actorrepository)
        {
            _actorRepository = actorrepository;
        }

        public async Task CreateActorAsync(CreateActorDto dto)
        {
            var actor = dto.ToActor();
            await _actorRepository.AddActorAsync(actor);
        }
        public async Task DeleteActorAsync(int id)
        {
            var exist = await _actorRepository.GetActorByIdAsync(id);
            if (exist == null) throw new Exception("actor not found");
            await _actorRepository.DeleteActorAsync(id);
        }
        public async Task<List<GetAllActorsDto>> GetAllActorsAsync()
        {
            var actors = await _actorRepository.GetAllActorsAsync();
            return actors.ToActor();
        }
        public async Task<GetActorByIdDto> GetActorByIdAsync(int id)
        {
            var actor = await _actorRepository.GetActorByIdAsync(id);
            return actor.ToActorWithMovies();
        }
        public async Task UpdateActorAsync(UpdateActorDto dto)
        {
            var actor = dto.ToActor();
            await _actorRepository.UpdateActorAsync(actor);
        }

        public async Task<List<GetAllActorsDto>> SearchActorsAsync(string name)
        {
            var actor = await _actorRepository.SearchByNameAsync(name);
            return actor.ToActor();
        }

    }
}
