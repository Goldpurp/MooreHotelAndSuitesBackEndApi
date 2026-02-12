using MooreHotelAndSuites.Application.Notifications;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Infrastructure.Notifications
{
    public class EventServiceNotificationHandler : INotificationChannelHandler
    {
        private readonly INotificationService _notify;
        public NotificationChannel Channel => NotificationChannel.EventService;

        public EventServiceNotificationHandler(INotificationService notify)
        {
            _notify = notify;
        }

        public async Task HandleAsync(OrderNotification n)
        {
            await _notify.BroadcastAsync("eventhall", new
            {
                type = "NEW_ORDER",
                orderId = n.OrderId,
                message = n.Message
            });
        }
    }
}
