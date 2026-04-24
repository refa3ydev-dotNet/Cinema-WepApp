using Business.DTOs.Bookings;

namespace Business.Managers.Bookings;

public interface IBookingManager
{
    Task<bool> ProcessCheckoutAsync(CheckoutDto dto,string userId);
    Task<SeatSelectionDto> GetSeatSelectionDataAsync(int scheduleId);
    // بنرجع IEnumerable<object> أو DTO مخصص للـ JSON
    Task<IEnumerable<object>> GetAvailableSchedulesForMovieAsync(int movieId);
    Task<List<MyTicketDto>> GetUserTicketsAsync(string userId);
}