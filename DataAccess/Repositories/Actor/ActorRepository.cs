using Core;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace DataAccess.Repositories.ACTOR
{ 
    public class ActorRepository: IActorRepository
    {
        private readonly MoviesDbContext _context;
        public ActorRepository (MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<Actor>GetActorByIdAsync(int id)
        {
            if (id > 0)
            {
                return await _context.Actors
                    .Include(x=>x.ActorMovies)
                    .ThenInclude(x=>x.Movie)
                    .ThenInclude(m=>m.CinemaMovies)
                    .ThenInclude(c=>c.Cinema)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                return null;
            }
        }
        public async Task<List<Actor>> GetAllActorsAsync()
        {
            if(_context.Actors != null)
            {

            return await _context.Actors.OrderBy(a=>a.FullName).ToListAsync();
            }
            else
            {
                return null;
            }
        }
        public async Task AddActorAsync(Actor actor)
        {
            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateActorAsync(Actor actor)
        {
            var Act = await _context.Actors.FindAsync(actor.Id);
            if(Act != null)
            {
                if (string.IsNullOrEmpty(actor.ProfilePicture))
                {
                    actor.ProfilePicture = Act.ProfilePicture;
                }
                _context.Entry(Act).CurrentValues.SetValues(actor);
            }
            else
            {
                var dbActor = await _context.Actors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == actor.Id);
                if(dbActor != null && string.IsNullOrEmpty(actor.ProfilePicture))
                {
                    actor.ProfilePicture = dbActor.ProfilePicture;
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteActorAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if(actor != null)
            {
                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
        public async Task<List<Actor>> SearchByNameAsync(string name)
        {
            return await _context.Actors.Where(x => x.FullName.Contains(name)).ToListAsync();
        }
        public async Task <int> GetActorsCountAsync()
        {
            return await _context.Actors.CountAsync();
        }
    }
}
