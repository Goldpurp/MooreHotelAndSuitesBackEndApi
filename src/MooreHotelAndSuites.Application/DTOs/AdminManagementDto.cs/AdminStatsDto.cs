namespace MooreHotelAndSuites.Application.DTOs.Admin
{
    public record AdminStatsDto(
        int TotalUsers,
        int TotalStaff,
        int TotalGuests,
        int ActiveBookings
    );
}
