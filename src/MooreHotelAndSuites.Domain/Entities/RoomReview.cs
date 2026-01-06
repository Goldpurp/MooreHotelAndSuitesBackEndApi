using System;
using System.ComponentModel.DataAnnotations;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class RoomReview
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string GuestName { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
