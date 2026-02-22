using MooreHotelAndSuites.Domain.Enums;

using MooreHotelAndSuites.Application.Notifications;

namespace MooreHotelAndSuites.Application.Notifications
{
    public interface INotificationChannelHandler
    {
        NotificationChannel Channel { get; }
        Task HandleAsync(OrderNotification notification);
    }
}

