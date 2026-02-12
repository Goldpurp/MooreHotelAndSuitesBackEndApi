using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Domain.Abstractions;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class ServiceOrder
    {
        public Guid Id { get; private set; }

        // Link to booking/guest
        public Guid? BookingId { get; private set; }   // NULL for event walk-in
        public int? GuestId { get; private set; }      // created later

        // Room info (for room service)
        public Guid? RoomId { get; private set; }
        public string? RoomNumber { get; private set; }

        // Customer info
        public string CustomerName { get; private set; } = null!;
public string PhoneNumber { get; private set; } = null!;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        // Order info
        public OrderSource Source { get; private set; }
        public OrderStatus Status { get; private set; }
        public List<OrderItem> Items { get; private set; } = new();

        public decimal TotalAmount => Items.Sum(x => x.Total);

        // Domain events
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

        private ServiceOrder() { }

        // ===== FACTORIES =====

        // Kitchen/Bar hotel guest order
        public static ServiceOrder CreateForHotelGuest(
            Booking booking,
            string name,
            string phone,
            OrderSource source)
        {
            if (source is OrderSource.Kitchen or OrderSource.Bar)
            {
                if (booking.Status != BookingStatus.CheckedIn &&
                    booking.Status != BookingStatus.Reserved)
                    throw new InvalidOperationException(
                        "Only checked-in or reserved guests can order kitchen/bar");
            }

            var order = new ServiceOrder
            {
                Id = Guid.NewGuid(),
                BookingId = booking.Id,
                GuestId = booking.GuestId,
                CustomerName = name,
                PhoneNumber = phone,
                Source = source,
                Status = OrderStatus.PendingPayment,
                RoomId = booking.RoomId,
                RoomNumber = booking.Room?.RoomNumber
            };

            order.AddDomainEvent(new OrderCreatedEvent(order.Id, source));

            return order;
        }

        // Event hall walk-in
        public static ServiceOrder CreateForEventWalkIn(string name, string phone)
        {
            var order = new ServiceOrder
            {
                Id = Guid.NewGuid(),
                CustomerName = name,
                PhoneNumber = phone,
                Source = OrderSource.EventHall,
                Status = OrderStatus.PendingPayment
            };

            order.AddDomainEvent(new OrderCreatedEvent(order.Id, OrderSource.EventHall));

            return order;
        }

        // Room service order
        public static ServiceOrder CreateRoomService(Booking booking, string name, string phone)
        {
            if (booking.Status != BookingStatus.CheckedIn)
                throw new InvalidOperationException(
                    "Only checked-in guests can use room service");

            var order = new ServiceOrder
            {
                Id = Guid.NewGuid(),
                BookingId = booking.Id,
                GuestId = booking.GuestId,
                CustomerName = name,
                PhoneNumber = phone,
                Source = OrderSource.RoomService,
                Status = OrderStatus.PendingPayment,
                RoomId = booking.RoomId,
                RoomNumber = booking.Room?.RoomNumber
            };

            order.AddDomainEvent(new OrderCreatedEvent(order.Id, OrderSource.RoomService));

            return order;
        }
      public static ServiceOrder CreateLaundry(
    Booking booking,
    string name,
    string phone)
{
    if (booking.Status != BookingStatus.CheckedIn &&
        booking.Status != BookingStatus.Reserved)
        throw new InvalidOperationException(
            "Only checked-in or reserved guests can use laundry");

    var order = new ServiceOrder
    {
        Id = Guid.NewGuid(),
        BookingId = booking.Id,
        GuestId = booking.GuestId,

        RoomId = booking.RoomId,
        RoomNumber = booking.Room?.RoomNumber,

        CustomerName = name,
        PhoneNumber = phone,

        Source = OrderSource.Laundry,
        Status = OrderStatus.PendingPayment
    };

    // ✅ THIS WAS MISSING
    order.AddDomainEvent(
        new OrderCreatedEvent(order.Id, OrderSource.Laundry));

    return order;
}


        // ===== BEHAVIORS =====
        public void AddLaundryItem(
        LaundryServiceType type,
        int quantity,
        string description)
        {
            var item = OrderItem.FromLaundry(type, quantity, description);
            Items.Add(item);
        }

        public void AddItem(MenuItem menu, int quantity)
        {
            if (quantity <= 0) throw new InvalidOperationException("Quantity must be greater than zero");
            var item = new OrderItem(menu, quantity);
            Items.Add(item);
        }

      public void ConfirmPayment(int guestId)
{
    if (Status != OrderStatus.PendingPayment)
        throw new InvalidOperationException(
            "Order is not awaiting payment");

    GuestId = guestId;
    Status = OrderStatus.Confirmed;

    AddDomainEvent(
        new OrderPaymentConfirmedEvent(Id, Source));
}

        public void MarkCreated()
        {
            AddDomainEvent(new OrderCreatedEvent(Id, Source));
        }
        public void MarkServed()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Payment not confirmed");
            Status = OrderStatus.Served;
            UpdatedAt = DateTime.UtcNow;
        }


        // ===== DOMAIN EVENTS =====
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
