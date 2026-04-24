namespace Business.DTOs.Bookings;

public class MyTicketDto
{
    public int BookingId{get;set;}
    public string MovieName{get;set;}
    public string PosterUrl{get;set;}
    public string CinemaName{get;set;}
    public string RoomName{get;set;}
    public DateTime StartTime{get;set;}
    public decimal TotalPrice{get;set;}
    public DateTime BookingDate{get;set;}
    public string Status{get;set;}
    public List<string> Seats { get; set; } = new List<string>();
}