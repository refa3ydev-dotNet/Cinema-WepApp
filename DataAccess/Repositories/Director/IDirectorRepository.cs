using Core.Entities;
namespace DataAccess.Repositories.DIRECTOR
{
    public interface IDirectorRepository
    {
        Task<Director> GetDirectorByIdAsync(int id);
        Task<List<Director>> GetAllDirectorsAsync();
        Task CreateDirectorAsync(Director director);
        Task UpdateDirectorAsync(Director director);
        Task DeleteDirectorAsync(int id);
        Task<List<Director>> GetDirectorByNameAsync(string name);
    }
}
