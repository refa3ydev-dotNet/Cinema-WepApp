using Business.DTOs.Directors;

namespace Business.Managers.Directors
{
    public interface IDirectorManager
    {
        Task CreateDirectorAsync(CreateDirectorDto dto);
        Task DeleteDirectorAsync(int id);
        Task<List<GetAllDirectorDto>> GetAllDirectorsAsync();
        Task<GetDirectorByIdDto> GetDirectorByIdAsync(int id);
        Task UpdateDirectorAsync(UpdateDirectorDto dto);
        public Task<List<GetAllDirectorDto>> SearchDirectorByNameAsync(string name);
    }
}
