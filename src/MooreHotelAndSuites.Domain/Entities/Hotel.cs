namespace MooreHotelAndSuites.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "MooreHotelAndSuites";
        public string Location { get; set; } = string.Empty;
        public List<Room> Rooms { get; set; } = new();
    }
}
