using MooreHotelAndSuites.Application.DTOs.Analytics;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
    public interface IAuditAnalyticsRepository
    {
        Task<int> CountByActionAsync(string action);
        Task<List<ActionCountDto>> ActionsPerDayAsync(DateTime from);
    }
}
