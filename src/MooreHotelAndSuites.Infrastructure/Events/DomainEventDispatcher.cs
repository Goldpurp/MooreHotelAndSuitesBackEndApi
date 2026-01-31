using MooreHotelAndSuites.Application.Interfaces.Events;
using MooreHotelAndSuites.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MooreHotelAndSuites.Infrastructure.Events
{
    public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _provider;

    public DomainEventDispatcher(IServiceProvider provider)
    {
        _provider = provider;
    }
   
    public async Task DispatchAsync<TEvent>(TEvent domainEvent)
    {
        var handlers = _provider
            .GetServices<IDomainEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(domainEvent);
        }
    }
}

}
