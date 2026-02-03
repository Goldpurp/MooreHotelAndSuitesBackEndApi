using MooreHotelAndSuites.Domain.Abstractions;

namespace MooreHotelAndSuites.Domain.Events
{
    public sealed class PaymentConfirmedDomainEvent : IDomainEvent
    {
        public Guid BookingId { get; }
        public DateTime OccurredOn { get; }

        public PaymentConfirmedDomainEvent(Guid bookingId)
        {
            BookingId = bookingId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
