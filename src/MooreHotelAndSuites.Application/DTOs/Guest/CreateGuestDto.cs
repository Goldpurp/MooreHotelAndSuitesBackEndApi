namespace MooreHotelAndSuites.Application.DTOs.Guests
{
    public class CreateGuestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
