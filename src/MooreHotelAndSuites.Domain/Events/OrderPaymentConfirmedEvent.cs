using MooreHotelAndSuites.Domain.Abstractions;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Domain.Events
{
    public class OrderPaymentConfirmedEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public OrderSource Source { get; }

        public DateTime OccurredOn { get; }

        public OrderPaymentConfirmedEvent(Guid id, OrderSource source)
        {
            OrderId = id;
            Source = source;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
