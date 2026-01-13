using Business.DTOs.Directors;
using Business.Mapping;
using DataAccess.Repositories.DIRECTOR;
namespace Business.Managers.Directors
{
    public class DirectorManager : IDirectorManager
    {
        private readonly IDirectorRepository _directorRepository;
        public DirectorManager(IDirectorRepository directorRepository)
        {
            _directorRepository = directorRepository;
        }

        public async Task CreateDirectorAsync(CreateDirectorDto dto )
        {
            await _directorRepository.CreateDirectorAsync(dto.ToEntity());
        }

        public async Task DeleteDirectorAsync(int id)
        {
            await _directorRepository.DeleteDirectorAsync(id);
        }

        public async Task<List<GetAllDirectorDto>> GetAllDirectorsAsync()
        {
            var dirs =await _directorRepository.GetAllDirectorsAsync() ;
            return dirs.ToDto();
        }

        public async Task<GetDirectorByIdDto> GetDirectorByIdAsync(int id)
        {
            var dir =await _directorRepository.GetDirectorByIdAsync(id);
            return dir.ToDto();
        }

        public async Task<List<GetAllDirectorDto>> SearchDirectorByNameAsync(string name)
        {
            var dirs =await _directorRepository.GetDirectorByNameAsync(name);
            return dirs.ToDto();
        }

        public Task UpdateDirectorAsync(UpdateDirectorDto dto)
        {
            var dir = dto.ToEntity();
            return _directorRepository.UpdateDirectorAsync(dir);
        }
    }
}
