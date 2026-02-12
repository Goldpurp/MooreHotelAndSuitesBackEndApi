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
    public DateTime CreatedAt { get; private set; }

    public DateTime CheckIn { get; private set; }
    public DateTime CheckOut { get; private set; }

    public Guid RoomId { get; private set; }
    public Room Room { get; private set; } = null!;

   public int GuestId { get; private set; }
    public Guest Guest { get; private set; } = null!;

    public string? UserAccountId { get; private set; } // optional



    public int Occupants { get; private set; }

    public BookingStatus Status { get; private set; } = BookingStatus.Pending;
    public decimal AmountPaid { get; private set; }
    public decimal TotalAmount => _payments.Sum(p => p.Amount);
    public decimal ExpectedAmount { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments;

    
    public void Reserve()
    {
        if (Status != BookingStatus.PaymentConfirmed)
            throw new InvalidOperationException(
                "Payment must be confirmed first");

        Status = BookingStatus.Reserved;
    }

public void AssignGuest(int guestId)
{
    if (guestId <= 0) 
        throw new InvalidOperationException("GuestId must be valid");

    if (GuestId != 0)
        throw new InvalidOperationException("Guest already assigned");

    GuestId = guestId;
}

 public void ValidateConfirmationInput(
    decimal amount,
    string inputGuestName)
{
    if (Status != BookingStatus.Pending)
        throw new InvalidOperationException("Booking is not pending");

    if (amount <= 0)
        throw new InvalidOperationException("Invalid amount");

    // name check will be done in controller using Guest entity
}

    public void ConfirmPayment(decimal amount)
    {
        AmountPaid += amount;
        Status = BookingStatus.PaymentConfirmed;

        AddDomainEvent(new PaymentConfirmedDomainEvent(Id));
    }

    public void MarkAsCheckedIn()
{
    if (Status != BookingStatus.Reserved)
        throw new InvalidOperationException(
            "Only reserved bookings can be checked in");

    if (DateTime.UtcNow.Date < CheckIn.Date)
        throw new InvalidOperationException(
            "Cannot check in before scheduled check-in date");

    Status = BookingStatus.CheckedIn;

    AddDomainEvent(new BookingCheckedInDomainEvent(Id));
}

 public void MarkAsCheckedOut()
{
    if (Status != BookingStatus.CheckedIn)
        throw new InvalidOperationException(
            "Only checked-in bookings can be checked out");

    Status = BookingStatus.CheckedOut;

    AddDomainEvent(new BookingCheckedOutDomainEvent(Id));
}



   public Payment AddPayment(
    decimal amount,
    string paymentMethod,
    string staffId,
    string guestFullName)
{
    if (amount <= 0)
        throw new InvalidOperationException("Payment amount must be greater than zero");

    if (Status != BookingStatus.Pending)
        throw new InvalidOperationException(
            "Only pending bookings can receive payment");
    var payment = new Payment(
        bookingId: Id,              // automatic
        amount: amount,
        paymentMethod: paymentMethod,
        payeeName: guestFullName,   // from guest
        staffId: staffId            // from logged user
    );

    _payments.Add(payment);

    ConfirmPayment(amount);

    return payment;
}

public static Booking Create(
    int guestId,
    DateTime checkIn,
    DateTime checkOut,
    int occupants,
    decimal roomPricePerNight)
{
    var nights = (checkOut - checkIn).Days;

    var booking = new Booking
    {
        Id = Guid.NewGuid(),
        GuestId = guestId,
        CheckIn = checkIn,
        CheckOut = checkOut,
        Occupants = occupants,
        Status = BookingStatus.Pending,
        CreatedAt = DateTime.UtcNow,

        ExpectedAmount = nights * roomPricePerNight
    };

  booking.AddDomainEvent(
    new BookingCreatedDomainEvent(booking.Id, guestId)
);

    return booking;
}




    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

}
