using MooreHotelAndSuites.Application.DTOs.Ledger;
using MooreHotelAndSuites.Application.DTOs.DailyStats;

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IOperationsService
{
    Task<List<LedgerEntryDto>> GetLedgerAsync();
    Task<DailyStatsDto> GetDailyStatsAsync();
}
}

