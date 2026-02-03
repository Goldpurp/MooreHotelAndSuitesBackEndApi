using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }


        // Basic info
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Pricing & capacity
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public double Size { get; set; }

        // Status & classification
        public RoomType RoomType { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        // Relations
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<RoomAmenity> RoomAmenities { get; set; }
        public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
        public ICollection<RoomReview> Reviews { get; set; } = new List<RoomReview>();

        // Ratings (calculated)
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
