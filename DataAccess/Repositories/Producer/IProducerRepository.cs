using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.PRODUCER
{
    public interface IProducerRepository
    {
        Task<Producer> GetProducerByIdAsync(int id);
        Task<List<Producer>> GetAllProducersAsync();
        Task AddProducerAsync(Producer producer);
        Task UpdateProducerAsync(Producer producer);
        Task DeleteProducerAsync(int id);
        Task<List<Producer>> SearchByNameAsync(string name);
        Task<int> GetProducersCountAsync();
    }
}
