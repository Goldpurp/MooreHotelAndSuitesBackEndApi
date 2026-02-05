using MooreHotelAndSuites.Domain.Abstractions;

namespace MooreHotelAndSuites.Application.Interfaces.Events
{
    public interface IDomainEventDispatcher
    {
     Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents);

    }
}
