namespace MooreHotelAndSuites.Application.DTOs.Rooms
{
    public class RoomDetailsDto
    {
        public Guid Id { get; set; }

        public string RoomNumber { get; set; } = default!;
        public string RoomName { get; set; } = default!;
        public string? Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public double Size { get; set; }
        public string RoomType { get; set; } = default!;
        public string Status { get; set; } = default!;

        public IEnumerable<string> Amenities { get; set; } = new List<string>();
        public IEnumerable<string> Images { get; set; } = new List<string>();

        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
