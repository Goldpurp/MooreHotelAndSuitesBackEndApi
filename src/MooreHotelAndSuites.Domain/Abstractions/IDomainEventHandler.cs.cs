using MooreHotelAndSuites.Domain.Events;

namespace MooreHotelAndSuites.Domain.Abstractions
{
    public interface IDomainEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent domainEvent);
    }
}

