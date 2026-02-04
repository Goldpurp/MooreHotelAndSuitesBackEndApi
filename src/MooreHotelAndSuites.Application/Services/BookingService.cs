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

var existing = await _repo.GetRecentPendingByGuestAsync(
    guestId,
    TimeSpan.FromMinutes(15));

if (existing != null)
{
    throw new InvalidOperationException(
        "Guest already has a pending booking within last 10 minutes");
}

    
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
        staffId: "SYSTEM",
        guestFullName: dto.GuestFullName
    );

    }

    await _repo.AddAsync(booking);

    return booking.Id;
}



public async Task<Guid> CreateDraftAsync(CreateBookingRequestDto dto)
{
    // Ensure guest exists (create if needed)
    var guestId = await _guestService.EnsureGuestAsync(
        dto.GuestFullName,
        dto.GuestEmail ?? string.Empty,
        dto.GuestPhoneNumber
    );

    // Create draft booking with guestId
    var booking = Booking.Create(
        roomId: dto.RoomId,
        checkIn: dto.CheckInDate,
        checkOut: dto.CheckOutDate,
        guestId: guestId
    );

    // Status is Pending by default (awaiting payment)
    await _repo.AddAsync(booking);

    return booking.Id;
}
public async Task<Booking?> FindPendingForConfirmationAsync(
    string? fullName,
    string? phone)
{
    // 1. Find guest first
    Guest? guest = null;

    if (!string.IsNullOrEmpty(fullName))
        guest = await _guestService.FindByNameAsync(fullName);

    if (guest == null && !string.IsNullOrEmpty(phone))
        guest = await _guestService.FindByPhoneAsync(phone);

    if (guest == null)
        return null;

    // 2. Then find booking by GuestId
    return await _repo.GetLastPendingByGuestIdAsync(guest.Id);
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
public async Task CheckOutAsync(Guid bookingId)
{
    var booking = await _repo.GetByIdAsync(bookingId);

    if (booking == null)
        throw new Exception("Booking not found");
 
   if (DateTime.UtcNow < booking.CheckOut)
    throw new InvalidOperationException("Cannot checkout before the scheduled date");

    booking.MarkAsCheckedOut();

    await _repo.UpdateAsync(booking);

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
