using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Domain.Abstractions;
using MooreHotelAndSuites.Application.Notifications;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Application.EventHandlers
{
public class OrderCreatedHandler : IDomainEventHandler<OrderCreatedEvent>
{
    private readonly NotificationRouter _router;

    public OrderCreatedHandler(NotificationRouter router)
    {
        _router = router;
    }

    public async Task HandleAsync(OrderCreatedEvent e)
    {
        var channel = e.Source switch
        {
            OrderSource.Kitchen => NotificationChannel.Kitchen,
            OrderSource.Bar => NotificationChannel.Bar,
            OrderSource.RoomService => NotificationChannel.RoomService,
            OrderSource.EventHall => NotificationChannel.EventService,
            OrderSource.Laundry => NotificationChannel.Laundry,
            _ => throw new Exception($"Unknown source: {e.Source}")
        };

        await _router.RouteAsync(new OrderNotification
        {
            OrderId = e.OrderId,
            Channel = channel,
            Message = $"New order from {e.Source}"
        });
    }
}

}
