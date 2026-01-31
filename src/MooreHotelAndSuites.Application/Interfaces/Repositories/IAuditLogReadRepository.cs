using MooreHotelAndSuites.Application.DTOs.Analytics;
using MooreHotelAndSuites.Application.DTOs.Ledger;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
    public interface IAuditLogReadRepository
{
    Task<List<LedgerEntryDto>> GetLedgerAsync();
    Task<int> CountByActionAsync(string action);
    Task<List<ActionCountDto>> CountActionsPerDayAsync(DateTime from);
}
}


