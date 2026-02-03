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
   private readonly IGuestService _guestService;
    public BookingService(
        IBookingRepository repo,
        IDomainEventDispatcher eventDispatcher,
        IGuestService guestService)
    {
        _repo = repo;
        _eventDispatcher = eventDispatcher;
        _guestService = guestService;
    }


   public async Task<Guid> CreateBookingAsync(CreateBookingRequestDto dto)
{
   
   var guestId = await _guestService.EnsureGuestAsync(
    dto.GuestFullName,
    dto.GuestEmail ?? string.Empty, // or throw if email is required
    dto.GuestPhoneNumber
);


    
    var booking = Booking.Create(
        roomId: dto.RoomId,
        checkIn: dto.CheckInDate,
        checkOut: dto.CheckOutDate,
        guestId: guestId
    );

    // Optional upfront payment
    if (dto.InitialPaymentAmount.HasValue)
    {
        booking.AddPayment(
            dto.InitialPaymentAmount.Value,
            dto.PaymentMethod ?? "Cash",
            dto.PayeeName ?? dto.GuestFullName,
            dto.AccountNumber,
            dto.BankName,
            staffId: "SYSTEM"
        );
    }

    await _repo.AddAsync(booking);

    return booking.Id;
}



public async Task<Guid> CreateDraftAsync(CreateBookingRequestDto dto)
{
    // Use 0 as placeholder guestId for draft bookings
    var booking = Booking.Create(
        dto.RoomId,
        dto.CheckInDate,
        dto.CheckOutDate,
        guestId: 0
    );

    await _repo.AddAsync(booking);

    return booking.Id;
}


public async Task CheckInAsync(Guid bookingId)
{
    var booking = await _repo.GetByIdAsync(bookingId);

    if (booking == null)
        throw new Exception("Booking not found");

    booking.MarkAsCheckedIn();

    await _repo.AddAsync(booking);

    await _eventDispatcher.DispatchAsync(booking.DomainEvents);
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
