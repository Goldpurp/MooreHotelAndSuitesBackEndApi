namespace MooreHotelAndSuites.Domain.Entities
{
    public class RoomImage
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsCover { get; set; }
        public int DisplayOrder { get; set; }
    }
}
