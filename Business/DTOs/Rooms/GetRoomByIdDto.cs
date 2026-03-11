using Core;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Rooms
{
    public class GetRoomByIdDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int SeatCount { get; set; }
        public int CinemaId { get; set; }
        public int SeatsPerRow { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
