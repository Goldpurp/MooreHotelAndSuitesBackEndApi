namespace MooreHotelAndSuites.Application.DTOs.Booking
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public Guid RoomId { get; set; }
        public string GuestId { get; set; } = string.Empty;

    }
}
