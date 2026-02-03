using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Domain.Abstractions;

namespace MooreHotelAndSuites.Domain.Entities
{
   public class Booking
{
    private Booking() { }

    public Guid Id { get; private set; }
    public string Reference { get; private set; } = string.Empty;

    public DateTime CheckIn { get; private set; }
    public DateTime CheckOut { get; private set; }

    public Guid RoomId { get; private set; }
    public Room Room { get; private set; } = null!;

   public int GuestId { get; private set; }



    
    public BookingStatus Status { get; private set; } = BookingStatus.Pending;
    public decimal AmountPaid { get; private set; }
    public decimal TotalAmount => _payments.Sum(p => p.Amount);

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments;

    
    public void Reserve()
    {
        if (Status != BookingStatus.PaymentConfirmed)
            throw new InvalidOperationException("Payment must be confirmed first");

        Status = BookingStatus.Reserved;
    }

   
    public void ConfirmPayment(decimal amount)
    {
        AmountPaid += amount;
        Status = BookingStatus.PaymentConfirmed;

        AddDomainEvent(new PaymentConfirmedDomainEvent(Id));
    }

    public void MarkAsCheckedIn()
    {
        if (Status != BookingStatus.PaymentConfirmed)
            throw new InvalidOperationException(
                "Only confirmed bookings can be checked in");

        Status = BookingStatus.CheckedIn;

        AddDomainEvent(new BookingCheckedInDomainEvent(Id));
    }

    public Payment AddPayment(
        decimal amount,
        string paymentMethod,
        string payeeName,
        string? accountNumber,
        string? bankName,
        string staffId)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Payment amount must be greater than zero");

        var payment = new Payment(
            Id,
            amount,
            paymentMethod,
            payeeName,
            accountNumber,
            bankName,
            staffId
        );

        _payments.Add(payment);

       
        ConfirmPayment(amount);

        return payment;
    }
public static Booking Create(
    Guid roomId,
    DateTime checkIn,
    DateTime checkOut,
    int guestId) // now int
{
    var booking = new Booking
    {
        Id = Guid.NewGuid(),
        Reference = $"BK-{Guid.NewGuid().ToString()[..8].ToUpper()}",
        RoomId = roomId,
        CheckIn = checkIn,
        CheckOut = checkOut,
        GuestId = guestId, // int
        Status = BookingStatus.Pending
    };

    booking.AddDomainEvent(new BookingCreatedDomainEvent(booking.Id, booking.GuestId));
    return booking;
}



    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

}
