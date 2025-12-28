using Business.DTOs.Producers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
