using MooreHotelAndSuites.Application.Notifications;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Infrastructure.Notifications
{
    public class LaundryNotificationHandler : INotificationChannelHandler
    {
        private readonly INotificationService _notify;
        public NotificationChannel Channel => NotificationChannel.Laundry;

        public LaundryNotificationHandler(INotificationService notify)
        {
            _notify = notify;
        }

        public async Task HandleAsync(OrderNotification n)
        {
            await _notify.BroadcastAsync("laundry", new
            {
                type = "NEW_ORDER",
                orderId = n.OrderId,
                message = n.Message
            });
        }
    }
}
