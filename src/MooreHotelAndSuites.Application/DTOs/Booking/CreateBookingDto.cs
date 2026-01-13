namespace MooreHotelAndSuites.Application.DTOs.Booking
{
    public class CreateBookingDto
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public Guid RoomId { get; set; }
        public Guid GuestId { get; set; }
    }
}
