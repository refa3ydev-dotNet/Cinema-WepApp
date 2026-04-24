namespace Business.DTOs.Bookings;

public class SeatSelectionDto
{
    public int ScheduleId { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public string PosterImg { get; set; }
    public DateTime StartTime { get; set; }
    public decimal TicketPrice { get; set; }
    
    public string CinemaName { get; set; }
    public string CinemaAddress { get; set; }
    public string RoomName { get; set; }
    public int SeatCount { get; set; }
    public int SeatPerRow { get; set; }
    
    public List<SeatDto> AllSeats { get; set; }
    public List<int> BookedSeatIds{ get; set; }
    
}