using MooreHotelAndSuites.Application.DTOs.DailyStats;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.DTOs.Ledger;
using MooreHotelAndSuites.Application.Interfaces.Services;
namespace MooreHotelAndSuites.Application.Services
{
 public class OperationsService : IOperationsService
{
    private readonly IBookingReadRepository _bookings;
    private readonly IAuditLogReadRepository _audit;

    public OperationsService(
        IBookingReadRepository bookings,
        IAuditLogReadRepository audit)
    {
        _bookings = bookings;
        _audit = audit;
    }

    public async Task<DailyStatsDto> GetDailyStatsAsync()
    {
        return new DailyStatsDto
        {
            Date = DateTime.UtcNow.Date,
            TotalBookings = await _bookings.CountTodayAsync(),
            ActiveBookings = await _bookings.CountActiveBookingsAsync()
        };
    }

    public Task<List<LedgerEntryDto>> GetLedgerAsync()
        => _audit.GetLedgerAsync();
}

}