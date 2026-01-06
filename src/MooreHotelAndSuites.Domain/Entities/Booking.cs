namespace MooreHotelAndSuites.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public int GuestId { get; set; }
        public Guest? Guest { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Reference { get; set; } = string.Empty;
    }
}
