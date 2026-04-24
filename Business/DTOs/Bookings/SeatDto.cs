namespace Business.DTOs.Bookings;

public class SeatDto
{
    public int Id { get; set; }
    public string Row { get; set; }
    public string Column { get; set; }
    public string SeatType { get; set; }
}