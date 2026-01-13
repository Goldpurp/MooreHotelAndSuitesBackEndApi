namespace MooreHotelAndSuites.Application.DTOs.Rooms
{
    public class CreateRoomDto
    {
        public string RoomNumber { get; set; } = default!;
        public string RoomName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public double Size { get; set; }
        public int RoomType { get; set; }

        public IReadOnlyCollection<Guid> AmenityIds { get; set; } = [];

    }
}
