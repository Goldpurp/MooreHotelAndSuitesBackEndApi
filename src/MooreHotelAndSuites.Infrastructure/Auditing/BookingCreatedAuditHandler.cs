using MooreHotelAndSuites.Domain.Abstractions;
using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Application.Interfaces.Auditing;

public sealed class BookingCreatedAuditHandler
    : IDomainEventHandler<BookingCreatedEvent>
{
    private readonly IAuditLogWriter _writer;

    public BookingCreatedAuditHandler(IAuditLogWriter writer)
    {
        _writer = writer;
    }

    public Task HandleAsync(BookingCreatedEvent evt)
    {
        return _writer.WriteAsync(
            evt.GuestId.ToString(),
            "BOOKING_CREATED",
            $"booking/{evt.BookingId}");
    }
}
