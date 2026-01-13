using MooreHotelAndSuites.Application.DTOs.Booking;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;

        public BookingService(IBookingRepository repo)
        {
            _repo = repo;
        }

        public async Task<BookingDto?> GetAsync(Guid id)
        {
            var booking = await _repo.GetByIdAsync(id);
            if (booking == null) return null;

            return new BookingDto
            {
                Id = booking.Id,
                Reference = booking.Reference,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                RoomId = booking.RoomId,
                GuestId = booking.GuestId
            };
        }

        public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                Reference = $"BK-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                RoomId = dto.RoomId,
                GuestId = dto.GuestId
            };

            await _repo.AddAsync(booking);

            return new BookingDto
            {
                Id = booking.Id,
                Reference = booking.Reference,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                RoomId = booking.RoomId,
                GuestId = booking.GuestId
            };
        }
    }
}
