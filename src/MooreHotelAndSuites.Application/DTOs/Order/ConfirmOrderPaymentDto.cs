
namespace MooreHotelAndSuites.Application.DTOs.Order
{
    public class ConfirmOrderPaymentDto
{
    public string CustomerName { get; set; }  = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
     public int GuestId { get; set; }
}
}