
using MediatR;


namespace MooreHotelAndSuites.Domain.Abstractions
{  public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}

