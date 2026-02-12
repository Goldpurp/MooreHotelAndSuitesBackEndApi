using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Abstractions;
namespace MooreHotelAndSuites.Application.EventHandlers.Orders
{
    public class OrderPaymentConfirmedHandler
        : IDomainEventHandler<OrderPaymentConfirmedEvent>
    {
        private readonly INotificationService _notify;

        public OrderPaymentConfirmedHandler(
            INotificationService notify)
        {
            _notify = notify;
        }

        public async Task HandleAsync(
            OrderPaymentConfirmedEvent e)
        {
            await _notify.BroadcastAsync("staff", new
            {
                type = "PAYMENT_CONFIRMED",
                orderId = e.OrderId,
                source = e.Source.ToString()
            });
        }
    }
}
