using Core.Entities;

namespace DataAccess.Repositories;

public interface IBookingRepository
{
    Task<bool> AreSeatsAvailableAsync(int scheduleId,List<int>  bookedSeatIds);
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<List<int>> GetBookedSeatIdsAsync(int scheduleId);
    Task<List<Booking>> GetUserBookingsAsync(string userId);
}