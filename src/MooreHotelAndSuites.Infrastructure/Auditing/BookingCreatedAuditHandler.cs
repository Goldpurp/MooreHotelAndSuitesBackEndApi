using MooreHotelAndSuites.Domain.Abstractions;
using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Application.Interfaces.Auditing;

public sealed class BookingCreatedAuditHandler
    : IDomainEventHandler<BookingCreatedDomainEvent>
{
    private readonly IAuditLogWriter _writer;

    public BookingCreatedAuditHandler(IAuditLogWriter writer)
    {
        _writer = writer;
    }

    public Task Handle(
        BookingCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        return _writer.WriteAsync(
            notification.GuestId.ToString(),
            "BOOKING_CREATED",
            $"booking/{notification.BookingId}");
    }
}
