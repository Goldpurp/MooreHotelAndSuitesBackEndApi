using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Application.DTOs.Booking;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Application.Interfaces.Events;



namespace MooreHotelAndSuites.Application.Services
{
    public class BookingService  : IBookingService
    
{
    private readonly IBookingRepository _repo;
    private readonly IDomainEventDispatcher _eventDispatcher;

    public BookingService(
        IBookingRepository repo,
        IDomainEventDispatcher eventDispatcher)
    {
        _repo = repo;
        _eventDispatcher = eventDispatcher;
    }


        public async Task<BookingDto> CreateAsync(CreateBookingDto dto, string guestId)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                Reference = $"BK-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                RoomId = dto.RoomId,
                GuestId = guestId
            };

            await _repo.AddAsync(booking);

               await _eventDispatcher.DispatchAsync(
              new BookingCreatedEvent(
            booking.Id,
            booking.GuestId,
            DateTime.UtcNow));

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

        public async Task<BookingDto?> GetAsync(Guid id)
    {
        var booking = await _repo.GetByIdAsync(id);
        if (booking == null)
            return null;

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


        public async Task CancelAsync(Guid id)
        {
            var booking = await _repo.GetByIdAsync(id);
            if (booking == null)
                throw new Exception("Booking not found");

            await _repo.DeleteAsync(booking);
        }

        private static string GenerateReference()
        {
            return $"BK-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6]}";
        }
    }
}
