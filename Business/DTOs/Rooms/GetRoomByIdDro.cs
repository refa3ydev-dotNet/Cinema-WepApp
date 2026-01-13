using Core;

namespace Business.DTOs.Rooms
{
    public class GetRoomByIdDro
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int SeatCount { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
    }
}
