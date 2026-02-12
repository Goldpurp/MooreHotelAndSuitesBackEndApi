using MooreHotelAndSuites.Application.Notifications;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Infrastructure.Notifications
{
    public class RoomServiceNotificationHandler : INotificationChannelHandler
    {
        private readonly INotificationService _notify;
        public NotificationChannel Channel => NotificationChannel.RoomService;

        public RoomServiceNotificationHandler(INotificationService notify)
        {
            _notify = notify;
        }

        public async Task HandleAsync(OrderNotification n)
        {
            await _notify.BroadcastAsync("roomservice", new
            {
                type = "NEW_ORDER",
                orderId = n.OrderId,
                message = n.Message
            });
        }
    }
}
