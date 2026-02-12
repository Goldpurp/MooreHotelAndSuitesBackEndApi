using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Application.Notifications
{
    public class OrderNotification
    {
        public Guid OrderId { get; set; }
        public NotificationChannel Channel { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
