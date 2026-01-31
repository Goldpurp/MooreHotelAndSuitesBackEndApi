using MooreHotelAndSuites.Application.DTOs.Analytics;


namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IAuditAnalyticsService
{
    Task<int> CountActionsAsync(string action);
    Task<List<ActionCountDto>> ActionsPerDayAsync(DateTime from);
}
}

