using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Application.DTOs.Booking;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Application.Interfaces.Events;


namespace MooreHotelAndSuites.Application.Services
{
    public class BookingService  : IBookingService
    
{
      private readonly IBookingRepository _repo;
    private readonly IRoomRepository _roomRepo;
    private readonly IDomainEventDispatcher _eventDispatcher;
    private readonly IGuestService _guestService;

    public BookingService(
        IBookingRepository repo,
        IDomainEventDispatcher eventDispatcher,
        IGuestService guestService, 
        IRoomRepository roomRepo) 
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
        _roomRepo = roomRepo ?? throw new ArgumentNullException(nameof(roomRepo));
    }


public async Task<BookingDto> CreateBookingAsync(CreateBookingRequestDto dto)
{
    // Ensure guest exists
    var guestId = await _guestService.EnsureGuestAsync(
        dto.GuestFullName,
        dto.GuestEmail ?? string.Empty,
        dto.GuestPhoneNumber
    );

    var room = await _roomRepo.GetByIdAsync(dto.RoomId);
if (room == null)
    throw new InvalidOperationException($"Room with ID {dto.RoomId} not found.");

    var booking = Booking.Create(
        guestId,
        dto.CheckInDate,
        dto.CheckOutDate,
        dto.Occupants,
        room.PricePerNight
    );

    await _repo.AddAsync(booking); // save first


return new BookingDto
{
    Id = booking.Id,
    CheckIn = booking.CheckIn,
    CheckOut = booking.CheckOut,
    RoomId = booking.RoomId,
    GuestId = booking.GuestId,
    ExpectedAmount = booking.ExpectedAmount,
    AmountPaid = booking.AmountPaid
};

}



public async Task<Booking?> FindPendingForConfirmationAsync(
    string? fullName,
    string? phone)
{
    if (string.IsNullOrWhiteSpace(fullName) &&
        string.IsNullOrWhiteSpace(phone))
        return null;

    Guest? guest = null;

    // PRIORITIZE PHONE – more unique
    if (!string.IsNullOrEmpty(phone))
        guest = await _guestService.FindByPhoneAsync(phone);

    // fallback to name
    if (guest == null && !string.IsNullOrEmpty(fullName))
        guest = await _guestService.FindByNameAsync(fullName);

    if (guest == null)
        return null;

    return await _repo.GetLastPendingByGuestIdAsync(guest.Id);
}



public async Task CheckInAsync(Guid bookingId)
{
    var booking = await _repo.GetByIdAsync(bookingId);

    if (booking == null)
        throw new Exception("Booking not found");

    
    if (booking.Status != BookingStatus.Reserved)
        throw new InvalidOperationException(
            "Only reserved bookings can be checked in");

    if (DateTime.UtcNow.Date < booking.CheckIn.Date)
        throw new InvalidOperationException(
            "Cannot check in before scheduled check-in date");

    booking.MarkAsCheckedIn();

    await _repo.UpdateAsync(booking);

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
