using Core.Entities;

namespace Business.Managers.Rooms
{
    public interface IRoomManager
    {
        Task AddRoom();
        Task DeleteRoom(Room room);
        Task UpdateRoom(Room room);
        Task<Room> GetRoomById(int id);
        Task<List<Room>> GetRooms();

    }
}
