using Core.Entities;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.ROOM
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MoviesDbContext _context;
        public RoomRepository(MoviesDbContext context)
        {
            _context = context;
        }
        public async Task AddRoomAsync(Room room)
        {
            if (room == null)
            {
                return;
            }
            _context.ChangeTracker.Clear();
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteRoomAsync(int id)
        {
            var exRoom = await _context.Rooms.FindAsync(id);
            if (exRoom != null)
            {
                
            _context.Rooms.Remove(exRoom);
            await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Room>> GetAllRoomsAsync(int cinemaId)
        {
            return await _context.Rooms.Where(x => x.CinemaId == cinemaId).OrderBy(x => x.IsDeleted).ToListAsync();
        }
        public async Task<Room> GetRoomByIdAsync(int id)
        {
            var exRoom = _context.Rooms
                .Include(r=>r.Seats)
                .Include(r=>r.MovieSchedules)
                .FirstOrDefault(x => x.Id == id&&!x.IsDeleted);
            return exRoom;
        }

        public async Task UpdateRoomAsync(Room room)
        {
            

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        private async Task<int> RoomsCount()
        {
            return await _context.Rooms.CountAsync();
        }
    }
}
