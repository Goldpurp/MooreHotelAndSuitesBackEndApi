namespace MooreHotelAndSuites.Application.DTOs.Booking
{
  public class BookingDto
{
    public Guid Id { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Occupants { get; set; }           // <-- new
    public decimal ExpectedAmount { get; set; }  // changed to decimal
    public decimal AmountPaid { get; set; }      // changed to decimal
    public Guid RoomId { get; set; }
    public int GuestId { get; set; }
}

}
