using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class Payment
    {
        // 🔹 EF Core needs this
        private Payment() { }

        // ✅ THIS IS THE MISSING CONSTRUCTOR
        public Payment(
            Guid bookingId,
            decimal amount,
            string paymentMethod,
            string payeeName,
            string? accountNumber,
            string? bankName,
            string staffId)
        {
            Id = Guid.NewGuid();
            BookingId = bookingId;

            Amount = amount;
            PaymentMethod = paymentMethod;
            PayeeName = payeeName;
            AccountNumber = accountNumber;
            BankName = bankName;

            ConfirmedByStaffId = staffId;
            PaidAt = DateTime.UtcNow;
            Status = PaymentStatus.Confirmed;
        }

        public Guid Id { get; private set; }
        public Guid BookingId { get; private set; }

        public decimal Amount { get; private set; }
        public string PaymentMethod { get; private set; } = string.Empty;
        public string PayeeName { get; private set; } = string.Empty;

        public string? AccountNumber { get; private set; }
        public string? BankName { get; private set; }

        public DateTime PaidAt { get; private set; }
        public string ConfirmedByStaffId { get; private set; } = string.Empty;

        public PaymentStatus Status { get; private set; }
    }
}
