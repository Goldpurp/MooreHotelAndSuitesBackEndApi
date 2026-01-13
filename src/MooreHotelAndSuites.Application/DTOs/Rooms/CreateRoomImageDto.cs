namespace MooreHotelAndSuites.Application.DTOs.Rooms
{
    public class CreateRoomImageDto
    {
        public string ImageUrl { get; set; } = default!;
        public bool IsCover { get; set; }
        public int DisplayOrder { get; set; }
    }
}
