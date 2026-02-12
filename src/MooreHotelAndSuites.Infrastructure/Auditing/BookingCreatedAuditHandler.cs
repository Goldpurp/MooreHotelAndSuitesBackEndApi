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

   public Task HandleAsync(BookingCreatedDomainEvent domainEvent)
{
    return _writer.WriteAsync(
        domainEvent.GuestId.ToString(),
        "BOOKING_CREATED",
        $"booking/{domainEvent.BookingId}");
}

}
