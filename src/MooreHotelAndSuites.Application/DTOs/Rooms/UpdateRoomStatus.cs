namespace MooreHotelAndSuites.Application.DTOs.Rooms
{
    public class UpdateRoomStatusDto
    {
       public Guid RoomId { get; set; }

        public int Status { get; set; }
    }
}
