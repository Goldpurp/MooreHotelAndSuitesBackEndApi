namespace MooreHotelAndSuites.Application.DTOs.Analytics
{
    public sealed class ActionCountDto
    {
        public string Action { get; init; } = string.Empty;
        public int Count { get; init; }
         public DateTime Date { get; init; }
    }
}
