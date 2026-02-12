using MooreHotelAndSuites.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using MooreHotelAndSuites.Application.Interfaces.Events;

namespace MooreHotelAndSuites.Infrastructure.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _provider;

        public DomainEventDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                var handlerType =
                    typeof(IDomainEventHandler<>)
                        .MakeGenericType(domainEvent.GetType());

                var handlers = _provider.GetServices(handlerType);

                foreach (var handler in handlers)
                {
                    var method = handlerType.GetMethod("HandleAsync");

                    if (method != null)
                    {
                        await (Task)method.Invoke(handler, new object[] { domainEvent })!;
                    }
                }
            }
        }
    }
}
