using MooreHotelAndSuites.Domain.Abstractions;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Domain.Events
{
    public class OrderCreatedEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public OrderSource Source { get; }

        public DateTime OccurredOn { get; }

        public OrderCreatedEvent(Guid id, OrderSource source)
        {
            OrderId = id;
            Source = source;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
