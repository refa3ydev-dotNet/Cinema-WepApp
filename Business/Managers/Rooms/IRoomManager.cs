using Business.DTOs.Rooms;
using Core.Entities;

namespace Business.Managers.Rooms
{
    public interface IRoomManager
    {
        Task AddRoomAsync(CreateRoomDto dto,int cinemaId);
        Task<bool> DeleteRoomAsync(int id);
        Task<bool> UpdateRoomAsync(UpdateRoomDto dto);
        Task<GetRoomByIdDto> GetRoomByIdAsync(int id);
        Task<List<GetAllRoomsDto>> GetCinemaRoomsAsync(int cinemaId);

    }
}
