using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Application.DTOs.Laundry
{
public class LaundryItemDto
{
    public LaundryServiceType Type { get; set; }
    public int Quantity { get; set; }

    public string Description { get; set; } = string.Empty;
}

}