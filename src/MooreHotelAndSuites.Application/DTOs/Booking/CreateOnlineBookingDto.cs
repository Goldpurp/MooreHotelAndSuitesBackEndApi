namespace MooreHotelAndSuites.Application.DTOs.Booking
{
    public class CreateOnlineBookingDto
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }

        
        public string? Email { get; set; }
        public string? UserAccountId { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Occupants { get; set; }
    }
}
