namespace MooreHotelAndSuites.Application.DTOs.Laundry
{
    public class CreateLaundryOrderDto
    {
       
        public required string CustomerName { get; set; }
        public required string PhoneNumber { get; set; }
        public required List<LaundryItemDto> Items { get; set; }

    }
}
