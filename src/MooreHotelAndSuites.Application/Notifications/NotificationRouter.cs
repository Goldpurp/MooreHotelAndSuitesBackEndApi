namespace MooreHotelAndSuites.Application.Notifications
{
    public class NotificationRouter
    {
        private readonly IEnumerable<INotificationChannelHandler> _handlers;

        public NotificationRouter(IEnumerable<INotificationChannelHandler> handlers)
        {
            _handlers = handlers;
        }

        public async Task RouteAsync(OrderNotification notification)
        {
            var handler = _handlers.FirstOrDefault(
                h => h.Channel == notification.Channel);

            if (handler != null)
                await handler.HandleAsync(notification);
        }
    }
}
