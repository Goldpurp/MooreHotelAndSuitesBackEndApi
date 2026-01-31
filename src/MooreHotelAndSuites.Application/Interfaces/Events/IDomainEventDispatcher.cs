namespace MooreHotelAndSuites.Application.Interfaces.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent domainEvent);
    }
}
