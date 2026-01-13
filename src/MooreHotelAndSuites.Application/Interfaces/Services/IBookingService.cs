using MooreHotelAndSuites.Application.DTOs.Booking;
    
    namespace MooreHotelAndSuites.Application.Interfaces.Services {

    public interface IBookingService
    {
        Task<BookingDto?> GetAsync(Guid id);
        Task<BookingDto> CreateAsync(CreateBookingDto dto);
        
    }
}