using Business.DTOs.Rooms;
using Core.Entities;

namespace Business.Mapping
{
    public static class RoomMapping
    {
        public static Room ToEntity(this UpdateRoomDto dto)
        {
            return new Room()
            {
                Id = dto.Id,
                RoomName = dto.RoomName,
                UpdatedAt = DateTime.Now,
                SeatCount = dto.SeatCount,
                CreatedAt = dto.CreatedAt,
            };
        }
        public static Room ToEntity(this CreateRoomDto dto,int cinemaId)
        {
            return new Room()
            {
                Id = dto.Id,
                RoomName = dto.RoomName,
                CreatedAt = DateTime.Now,
                SeatCount = dto.SeatCount,
                CinemaId = cinemaId,
                SeatsPerRow = dto.SeatsPerRow,
                IsDeleted = false

            };
        }

        public static List<GetAllRoomsDto> ToDto(this List<Room> room)
        {
            if (room == null) return null;
            return room.Select(room => new GetAllRoomsDto
            {
                Id = room.Id,
                RoomName = room.RoomName,
                UpdatedAt = room.UpdatedAt,
                SeatCount = room.SeatCount,
                
                CreatedAt = room.CreatedAt,
                IsDeleted = room.IsDeleted,
                DeletedAt = room.DeletedAt,
            }).ToList();
        }
        public static GetRoomByIdDto ToDto(this Room room)
        {
            return new GetRoomByIdDto()
            {
                Id = room.Id,
                RoomName = room.RoomName,
                UpdatedAt = room.UpdatedAt,
                SeatCount = room.SeatCount,
                CreatedAt = room.CreatedAt,
                IsDeleted = room.IsDeleted,
                DeletedAt = room.DeletedAt,
                CinemaId = room.CinemaId
            };
        }

    }
}
