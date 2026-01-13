using Core;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Rooms
{
    internal class UpdateRoomDto
    {
        [Key]
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int SeatCount { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }

    }
}
