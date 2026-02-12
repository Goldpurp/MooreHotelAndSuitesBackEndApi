using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Application.DTOs.Order
{
   public class CreateOrderDto
{
    public OrderSource Source { get; set; }

    // Optional for non-account users
    public string? CustomerName { get; set; }
    public string? PhoneNumber { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}


}