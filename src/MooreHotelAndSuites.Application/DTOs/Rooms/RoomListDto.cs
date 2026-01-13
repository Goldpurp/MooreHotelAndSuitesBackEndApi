namespace MooreHotelAndSuites.Application.DTOs.Rooms
{
    public class RoomListDto
    {
        public Guid Id { get; set; }

        public string RoomNumber { get; set; } = default!;
        public string RoomName { get; set; } = default!;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public double AverageRating { get; set; }
        public string CoverImageUrl { get; set; } = default!;
    }
}
