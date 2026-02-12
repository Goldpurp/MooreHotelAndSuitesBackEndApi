

namespace MooreHotelAndSuites.Application.DTOs.Booking
{
  public class CreateWalkInBookingDto
{
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Occupants { get; set; }
    public decimal InitialPayment { get; set; }
    public required string PaymentMethod { get; set; }
}


}