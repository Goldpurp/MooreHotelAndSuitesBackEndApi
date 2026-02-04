using MooreHotelAndSuites.Domain.Abstractions;

namespace MooreHotelAndSuites.Domain.Events
{
    public sealed class BookingCheckedOutDomainEvent : IDomainEvent
    {
        public BookingCheckedOutDomainEvent(Guid bookingId)
        {
            BookingId = bookingId;
            OccurredOn = DateTime.UtcNow;
        }

        public Guid BookingId { get; }
        public DateTime OccurredOn { get; }
    }
}
