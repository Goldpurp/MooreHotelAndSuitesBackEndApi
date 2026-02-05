using MooreHotelAndSuites.Domain.Abstractions;

namespace MooreHotelAndSuites.Domain.Events
{
    public sealed class BookingCreatedDomainEvent : IDomainEvent
{
    public Guid BookingId { get; }
   public int GuestId { get; }

    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public BookingCreatedDomainEvent(Guid bookingId, int guestId)
    {
        BookingId = bookingId;
        GuestId = guestId;
        OccurredOn = DateTime.UtcNow;
    }
}
}
