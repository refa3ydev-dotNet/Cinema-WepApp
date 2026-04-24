using Business.DTOs.Bookings;
using Core.Entities;
using Core.Entities.Relations;
using DataAccess.Repositories;
using DataAccess.Repositories.Schedule;

namespace Business.Managers.Bookings;

public class BookingManager:IBookingManager
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMovieScheduleRepository _movieScheduleRepository;

    public BookingManager(IBookingRepository bookingRepository, IMovieScheduleRepository movieScheduleRepository)
    {
        _bookingRepository = bookingRepository;
        _movieScheduleRepository = movieScheduleRepository;
    }
    public async Task<bool> ProcessCheckoutAsync(CheckoutDto dto, string userId)
    {
        if (string.IsNullOrEmpty(dto.SelectedSeatIds)) return false;

        var seatIds = dto.SelectedSeatIds.Split(',')
            .Select(int.Parse).ToList();
        
        var isAvailable= await _bookingRepository.AreSeatsAvailableAsync(dto.ScheduleId, seatIds);
        if (!isAvailable) return false;

        var schedule = await _movieScheduleRepository.GetScheduleWithDetailsByIdAsync(dto.ScheduleId);
        if (schedule == null) return false;
        
        
        decimal totalPrice = seatIds.Count*schedule.Price;

        var booking = new Booking
        {
            MovieScheduleId = dto.ScheduleId,
            UserId = userId,
            TotalPrice = totalPrice,
            BookingDate = DateTime.Now,
            Status = "Confirmed",
            BookingSeats = seatIds.Select(id => new BookingSeat
            {
                SeatId = id,
                PriceAtBooking = schedule.Price
            }).ToList()
        };
        await _bookingRepository.CreateBookingAsync(booking);
        return isAvailable;
    }

    public async Task<SeatSelectionDto> GetSeatSelectionDataAsync(int scheduleId)
    {
        var schedule = await _movieScheduleRepository.GetScheduleWithDetailsByIdAsync(scheduleId);
        if (schedule == null) return null;

        var bookedSearIds = await _bookingRepository.GetBookedSeatIdsAsync(scheduleId);
        return new SeatSelectionDto()
        {
            ScheduleId = schedule.Id,
            MovieId = schedule.MovieId,
            MovieTitle = schedule.Movie.Name,
            PosterImg = schedule.Movie.PosterImg ?? "",
            StartTime = schedule.StartDate,
            TicketPrice = schedule.Price,
            CinemaName = schedule.Cinema.Name,
            CinemaAddress = schedule.Cinema.Address ?? "N/A", 
            RoomName = schedule.Room.RoomName,
            SeatCount = schedule.Room.SeatCount,
            SeatPerRow = schedule.Room.SeatsPerRow,
            BookedSeatIds = bookedSearIds,
            AllSeats = schedule.Room.Seats.Select(s => new SeatDto
            {
                Id = s.Id,
                Row = s.Row,
                Column = s.Column.ToString(), 
                SeatType = s.SeatsType
            }).ToList()
        };
    }
    public async Task<IEnumerable<object>> GetAvailableSchedulesForMovieAsync(int movieId)
    {
        var schedules = await _movieScheduleRepository.GetActiveSchedulesByMovieIdAsync(movieId);

        // الـ Manager هو اللي بيقرر شكل الداتا اللي هتروح للـ UI
        return schedules.Select(s => new {
            id = s.Id,
            time = s.StartDate.ToString("hh:mm tt, dd MMM"),
            roomName = s.Room.RoomName,
            price = s.Price
        });
    }

    public async Task<List<MyTicketDto>> GetUserTicketsAsync(string userId)
    {
        var bookings= await _bookingRepository.GetUserBookingsAsync(userId);
        return bookings.Select(b => new MyTicketDto
        {
            BookingId = b.Id,
            MovieName = b.MovieSchedule.Movie.Name,
            PosterUrl = b.MovieSchedule.Movie.PosterImg,
            CinemaName = b.MovieSchedule.Cinema.Name,
            RoomName = b.MovieSchedule.Room.RoomName,
            StartTime = b.MovieSchedule.StartDate,
            TotalPrice = b.TotalPrice,
            BookingDate = b.BookingDate,
            Status = b.Status,
            Seats = b.BookingSeats.Select(bs => $"{bs.Seat.Row}{bs.Seat.Column}").ToList()
        }).ToList();
    }
}