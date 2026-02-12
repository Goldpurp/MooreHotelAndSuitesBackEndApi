using MooreHotelAndSuites.Application.DTOs.Order;
using MooreHotelAndSuites.Application.DTOs.Laundry;

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<(Guid id, decimal amount)> CreateOrderAsync(CreateOrderDto dto);

        Task<(Guid id, decimal amount)> CreateLaundryOrderAsync(
            CreateLaundryOrderDto dto);

        Task ConfirmPaymentAsync(ConfirmOrderPaymentDto dto);

        Task MarkServedAsync(ServeOrderDto dto);
    }
}
