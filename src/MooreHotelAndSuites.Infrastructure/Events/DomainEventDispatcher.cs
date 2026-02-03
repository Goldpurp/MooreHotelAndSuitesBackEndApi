using MediatR;
using MooreHotelAndSuites.Application.Interfaces.Events;
using MooreHotelAndSuites.Domain.Abstractions;

namespace MooreHotelAndSuites.Infrastructure.Events
{
    public sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
    }
}
