using Core;

namespace DataAccess.Repositories.ACTOR
{
    public interface IActorRepository
    {
        Task<Actor> GetActorByIdAsync(int id);
        Task<List<Actor>> GetAllActorsAsync();
        Task AddActorAsync(Actor actor);
        Task UpdateActorAsync(Actor actor);
        Task DeleteActorAsync(int id);
        Task<List<Actor>> SearchByNameAsync(string name);
        Task<int> GetActorsCountAsync();
    }
}
