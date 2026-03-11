using Core;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.Rooms
{
    public class CreateRoomDto:IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Room name is required (e.g., VIP Hall 1)")]
        [Display(Name = "Room Name")]
        public string RoomName { get; set; }
        [Required(ErrorMessage = "Please specify the seat count")]
        [Range(10, 500, ErrorMessage = "Seat count must be between 10 and 500")]
        public int SeatCount { get; set; }
        [Required(ErrorMessage = "Please specify how many seats in each row")]
        [Range(5, 50, ErrorMessage = "Row capacity must be between 5 and 50")]
        public int SeatsPerRow { get; set; }
        public int CinemaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SeatsPerRow > SeatCount)
            {
                yield return new ValidationResult(
                    "Error: Seats per row cannot be greater than the total seat capacity!",
                    new[] { nameof(SeatsPerRow) }
                    );
            }
        }
    }
}
