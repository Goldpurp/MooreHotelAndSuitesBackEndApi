using MooreHotelAndSuites.Domain.Abstractions;

namespace MooreHotelAndSuites.Domain.Events
{
    public sealed class BookingCheckedInDomainEvent : IDomainEvent
    {
        public Guid BookingId { get; }
        public DateTime OccurredOn { get; }

        public BookingCheckedInDomainEvent(Guid bookingId)
        {
            BookingId = bookingId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
