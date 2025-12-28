using Business.DTOs.Actors;
using Business.DTOs.Producers;
using Business.Mapping;
using DataAccess.Repositories.PRODUCER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers.Producers
{
    public class ProducersManager: IProducersManager
    {
        private readonly IProducerRepository _producerRepository;

        public ProducersManager(IProducerRepository producerRepository)
        {
            _producerRepository = producerRepository;
        }

        public async Task CreateProducerAsync(CreateProducerDto dto)
        {
            var producer = dto.ToEntity();
            await _producerRepository.AddProducerAsync(producer);
        }


        public async Task DeleteProducerAsync(int id)
        {
            var exist = await _producerRepository.GetProducerByIdAsync(id);
            if (exist == null) throw new Exception("actor not found");
            await _producerRepository.DeleteProducerAsync(id);
        }


        public async Task<List<GetAllProducersDto>> GetAllProducersAsync()
        {
            var producers = await _producerRepository.GetAllProducersAsync();
            return producers.ToDto();
        }


        public async Task<GetProducerByIdDto> GetProducerByIdAsync(int id)
        {
            var produser = await _producerRepository.GetProducerByIdAsync(id);
            return produser.ToProducerWithMovies();
        }


        public async Task UpdateProducerAsync(UpdateProducerDto dto)
        {
            var producer = dto.ToEntity();
            await _producerRepository.UpdateProducerAsync(producer);
        }

    }
}
