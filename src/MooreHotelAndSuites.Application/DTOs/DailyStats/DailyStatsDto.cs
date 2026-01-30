namespace MooreHotelAndSuites.Application.DTOs.DailyStats;

public sealed class DailyStatsDto
{
    public DateTime Date { get; init; }

    public int TotalBookings { get; init; }
    public int ActiveBookings { get; init; }

    public int NewGuests { get; init; }

    public decimal Revenue { get; init; }
}
