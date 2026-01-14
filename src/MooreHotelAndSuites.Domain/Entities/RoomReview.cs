using System;
using System.ComponentModel.DataAnnotations;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class RoomReview
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }

  
        public Room Room { get; set; } = null!;

        public int Rating { get; set; }
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
