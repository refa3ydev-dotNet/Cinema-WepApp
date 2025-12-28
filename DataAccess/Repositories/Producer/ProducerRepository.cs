using DataAccess.Contexts;
using Core;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories.PRODUCER
{
    public class ProducerRepository: IProducerRepository
    {
        private readonly MoviesDbContext _context;

        public ProducerRepository(MoviesDbContext context)
        {
            _context = context;
        }
        public async Task<Producer> GetProducerByIdAsync(int id)
        {
            if (id > 0)
            {
                return await _context.Producers
     .Include(x => x.Movies)
     .ThenInclude(m => m.CinemaMovies)
     .ThenInclude(c => c.Cinema)
     .FirstOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                return null;
            }
        }
        public async Task<List<Producer>> GetAllProducersAsync()
        {
            if (_context.Producers!=null)
            {
                return await _context.Producers.ToListAsync();
            }
            else
            {
                return null;
            }

        }
        public async Task AddProducerAsync(Producer producer)
        {
            _context.Producers.Add(producer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProducerAsync(Producer producer)
        {
            var prod = await _context.Producers.FindAsync(producer.Id);
            if (prod != null)
            {
                if (!string.IsNullOrEmpty(prod.ProfilePicture) &&producer.ProfilePicture != prod.ProfilePicture)
                {
                    var OldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Producers", Path.GetFileName(prod.ProfilePicture));
                    if (File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                    }
                }
                _context.Entry(prod).CurrentValues.SetValues(producer);
            }
            else
            {
                var dbProd= await _context.Producers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == producer.Id);
                if (dbProd != null && string.IsNullOrEmpty(producer.ProfilePicture))
                {
                    producer.ProfilePicture = dbProd.ProfilePicture;
                }

            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProducerAsync(int id)
        {
            var prod=await _context.Producers.FindAsync(id);
            if (prod != null)
            {
            _context.Producers.Remove(prod);

            }
            else
            {
                return;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Producer>> SearchByNameAsync(string name)
        {
            return await _context.Producers.Where(x => x.FullName.Contains(name)).ToListAsync();
        }
        public async Task<int> GetProducersCountAsync()
        {
            return await _context.Producers.CountAsync();
        }
    }
}
