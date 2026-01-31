namespace MooreHotelAndSuites.Domain.Events
{
    public sealed class BookingCreatedEvent
    {
        public Guid BookingId { get; }
        public string GuestId { get; }
        public DateTime OccurredOn { get; }

        public BookingCreatedEvent(
            Guid bookingId,
            string guestId,
            DateTime occurredOn)
        {
            BookingId = bookingId;
            GuestId = guestId;
            OccurredOn = occurredOn;
        }
    }
}
