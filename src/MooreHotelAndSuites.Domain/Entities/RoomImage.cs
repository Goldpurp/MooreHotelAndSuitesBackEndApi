using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Domain.Entities
{
    public class RoomImage
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }

      
        public Room Room { get; set; } = null!;

        public string ImageUrl { get; set; } = string.Empty;
        public bool IsCover { get; set; }
        public int DisplayOrder { get; set; }
    }
}
