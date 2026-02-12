namespace MooreHotelAndSuites.Domain.Abstractions
{
    public interface IDomainEventHandler<TEvent>
        where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent);
    }
}
