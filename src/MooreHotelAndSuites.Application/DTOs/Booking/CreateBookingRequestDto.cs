namespace MooreHotelAndSuites.Application.DTOs.Booking
{
    public sealed class CreateBookingRequestDto
    {
        public Guid RoomId { get; init; }

        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }

        public string GuestFullName { get; init; } = string.Empty;
        public string GuestPhoneNumber { get; init; } = string.Empty;
        public string? GuestEmail { get; init; }

        // Optional upfront payment (manual)
        public decimal? InitialPaymentAmount { get; init; }
        public string? PaymentMethod { get; init; } // Cash, Transfer, POS
        public string? PayeeName { get; init; }
        public string? AccountNumber { get; init; }
        public string? BankName { get; init; }
    }
}
