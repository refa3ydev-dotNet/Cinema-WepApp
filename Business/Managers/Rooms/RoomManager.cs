using Business.DTOs.Rooms;
using Business.Mapping;
using Core.Entities;
using DataAccess.Repositories.ROOM;

namespace Business.Managers.Rooms
{

    public class RoomManager : IRoomManager
    {
        private readonly IRoomRepository _roomRepository;
        public RoomManager(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task AddRoomAsync(CreateRoomDto dto,int cinemaId)
        {
            var roomEntity = dto.ToEntity(cinemaId);
            roomEntity.Seats = GenerateSeats(dto.SeatCount, dto.SeatsPerRow);
            await _roomRepository.AddRoomAsync(roomEntity);
        }

        public async Task<bool> DeleteRoomAsync(int id)

        {
            var existingRoom = await _roomRepository.GetRoomByIdAsync(id);
            if (existingRoom == null)
            {
                return false;
            }
            bool hasFutureSchedules=existingRoom.MovieSchedules.Any(s=>s.StartDate>=DateTime.Now);

            if(hasFutureSchedules)
            {
                return false;
            }
            existingRoom.IsDeleted = true;
            existingRoom.DeletedAt = DateTime.Now;
            await _roomRepository.UpdateRoomAsync(existingRoom);
            return true;
            
        }

        public async Task<List<GetAllRoomsDto>> GetCinemaRoomsAsync(int id)
        {
            var rooms = await _roomRepository.GetAllRoomsAsync(id);
            // Explicitly specify the type argument so the compiler can resolve AsyncEnumerable.ToListAsync<T>
            return rooms.ToDto().ToList();
        }

        public async Task<GetRoomByIdDto> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            return room?.ToDto();

        }

        public async Task<bool> UpdateRoomAsync(UpdateRoomDto dto)
        {
            var existingRoom=await _roomRepository.GetRoomByIdAsync(dto.Id);
            if (existingRoom==null)
            {
                return false;
            }

                existingRoom.RoomName = dto.RoomName;
                existingRoom.UpdatedAt=DateTime.Now;
                if (dto.SeatCount != existingRoom.SeatCount || dto.SeatsPerRow != existingRoom.SeatsPerRow)
                {
                    bool hasFutureSchedules = existingRoom.MovieSchedules!=null&& existingRoom.MovieSchedules
                        .Any(s=>s.StartDate>=DateTime.Now);
                    if (hasFutureSchedules)
                    {
                        return false;
                    }
                    existingRoom.SeatCount=dto.SeatCount;   
                    existingRoom.SeatsPerRow=dto.SeatsPerRow;

                    if (existingRoom.Seats != null)
                    {
                        existingRoom.Seats.Clear();
                    }
                    
                existingRoom.Seats=GenerateSeats(dto.SeatCount, dto.SeatsPerRow);
                }
                await _roomRepository.UpdateRoomAsync(existingRoom);
                return true;

        }
        private List<Seat> GenerateSeats(int seatCount, int seatsPerRow)
        {
            var seats =new List<Seat>();
            for (int i = 0; i < seatCount; i++)
            {
                int rowIndex = i / seatsPerRow;
                int columnNumber = (i % seatsPerRow) + 1;
                string rowLetter = GetRowLetter(rowIndex);

                seats.Add(new Seat
                {
                    Row = rowLetter,
                    Column = columnNumber,
                    SeatsType = "Standard",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                });
            }
            return seats;
        }
        private string GetRowLetter(int rowIndex)
        {
            const string rowLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if(rowIndex < 26)
            {
                return rowLetters[rowIndex].ToString();

            }
            return rowLetters[(rowIndex/26) - 1].ToString() + rowLetters[rowIndex%26].ToString();
        }
    }
}
