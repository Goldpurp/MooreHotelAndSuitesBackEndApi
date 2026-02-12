using MooreHotelAndSuites.Application.DTOs.Booking;
using MooreHotelAndSuites.Domain.Entities;
    namespace MooreHotelAndSuites.Application.Interfaces.Services {

   public interface IBookingService
{
   Task<BookingDto> CreateBookingAsync(CreateBookingRequestDto dto);

    Task<Booking?> FindPendingForConfirmationAsync(
    string? fullName,
    string? phone);
    Task CheckInAsync(Guid bookingId);
     Task CheckOutAsync(Guid bookingId);
    Task<BookingDto?> GetAsync(Guid id);

    Task CancelAsync(Guid id);
}

}