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
                throw new ArgumentNullException(nameof(room));
            }
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteRoomAsync(int id)
        {
            var exRoom = _context.Rooms.Find(id);
            if (exRoom == null)
            {
                throw new ArgumentNullException(nameof(exRoom));
            }
            _context.Rooms.Remove(exRoom);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            if (RoomsCount() == Task.FromResult(0))
            {
                throw new ArgumentNullException(nameof(GetAllRoomsAsync));
            }
            return await _context.Rooms.ToListAsync();
        }
        public async Task<Room> GetRoomByIdAsync(int id)
        {
            var exRoom = _context.Rooms.FirstOrDefault(x => x.Id == id);
            if (exRoom == null)
            {
                throw new ArgumentNullException(nameof(exRoom));
            }
            return exRoom;
        }

        public async Task UpdateRoomAsync(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        private async Task<int> RoomsCount()
        {
            return await _context.Rooms.CountAsync();
        }
    }
}
