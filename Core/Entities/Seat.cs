using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class Seat : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public string Row { get; set; }
        [Required]
        public int Column { get; set; }
        public string SeatsType { get; set; }= "Standard";

        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
    }
}
