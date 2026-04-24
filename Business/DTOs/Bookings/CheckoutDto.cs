namespace Business.DTOs.Bookings;

public class CheckoutDto
{
    public int ScheduleId { get; set; }
    public string SelectedSeatIds{get;set;}=string.Empty;
}