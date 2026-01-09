using Business.DTOs.Producers;

namespace Business.Managers.Producers
{
    public interface IProducersManager
    {
        Task CreateProducerAsync(CreateProducerDto dto);
        Task DeleteProducerAsync(int id);
        Task<List<GetAllProducersDto>> GetAllProducersAsync();
        Task<GetProducerByIdDto> GetProducerByIdAsync(int id);
        Task UpdateProducerAsync(UpdateProducerDto dto);
    }
}
