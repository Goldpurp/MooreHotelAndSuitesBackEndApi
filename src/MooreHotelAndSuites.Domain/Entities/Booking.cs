using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }
        public Room? Room { get; set; }

    
        public string GuestId { get; set; } = string.Empty;

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Reference { get; set; } = string.Empty;
    }
}
