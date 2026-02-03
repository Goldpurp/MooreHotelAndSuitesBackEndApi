using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Abstractions;


namespace MooreHotelAndSuites.Application.EventHandlers
{
   public sealed class PaymentConfirmedEventHandler
    : IDomainEventHandler<PaymentConfirmedDomainEvent>
{
    private readonly IAuditLogRepository _auditRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly IEmailService _emailService;
    private readonly IGuestService _guestService;
    private readonly IIdentityLookupService _identity;

    public PaymentConfirmedEventHandler(
        IAuditLogRepository auditRepo,
        IBookingRepository bookingRepo,
        IEmailService emailService,
        IIdentityLookupService identity,
        IGuestService guestService)
    {
        _auditRepo = auditRepo;
        _bookingRepo = bookingRepo;
        _emailService = emailService;
        _guestService = guestService;
       _identity = identity;
    }
public async Task Handle(
    PaymentConfirmedDomainEvent notification,
    CancellationToken cancellationToken)
{
   var booking = await _bookingRepo.GetByIdAsync(notification.BookingId);

if (booking is null)
    return;

booking.Reserve();
await _bookingRepo.UpdateAsync(booking);

// get guest from domain
var guest = await _guestService.GetByIdAsync(booking.GuestId);

if (guest is null || string.IsNullOrEmpty(guest.Email))
    return;

try
{
    await _emailService.SendAsync(
        to: guest.Email,
        subject: $"Payment Receipt - {booking.Reference}",
        body: BuildReceiptEmail(booking, guest) // pass guest
    );
}
catch
{
    await _auditRepo.AddAsync(new AuditLog
    {
        Id = Guid.NewGuid(),
        UserId = booking.GuestId.ToString(),
        Entity = nameof(Booking),
        Action = "EMAIL_FAILED",
        Method = "DomainEvent",
        Path = $"booking/{booking.Id}",
        StatusCode = 500,
        OccurredAt = DateTime.UtcNow
    });
}

}




        private static string BuildReceiptEmail(Booking booking, Guest guest) =>
        $@"Dear {guest.FullName},

        We have successfully confirmed your payment.

        BOOKING DETAILS
        Reference: {booking.Reference}
        Check-In: {booking.CheckIn:dddd, dd MMM yyyy}
        Check-Out: {booking.CheckOut:dddd, dd MMM yyyy}

        Your reservation is now CONFIRMED and guaranteed.

        We look forward to welcoming you to Moore Hotel & Suites.

        Regards,  
        Moore Hotel Team";


            }
}
