using MooreHotelAndSuites.Application.DTOs.Booking;

    namespace MooreHotelAndSuites.Application.Interfaces.Services {

   public interface IBookingService
{
   Task<Guid> CreateBookingAsync(CreateBookingRequestDto dto);

    Task<Guid> CreateDraftAsync(CreateBookingRequestDto dto);

    Task CheckInAsync(Guid bookingId);

    Task<BookingDto?> GetAsync(Guid id);

    Task CancelAsync(Guid id);
}

}