namespace MooreHotelAndSuites.Application.DTOs.Payments
{
    public class ConfirmPaymentDto
    {
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        // Staff can use ANY of these to match the correct booking
        public string? GuestFullName { get; set; }

        public string? GuestPhoneNumber { get; set; }

        public Guid? BookingId { get; set; }   // optional direct match
    }
}
