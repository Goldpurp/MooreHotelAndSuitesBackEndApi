using MediatR;

namespace MooreHotelAndSuites.Domain.Abstractions
{
    public interface IDomainEventHandler<TEvent>
        : INotificationHandler<TEvent>
        where TEvent : IDomainEvent
    {
    }
}
