using MooreHotelAndSuites.Application.DTOs.Analytics;
public interface IAnalyticsService
{
    Task<AnalyticsOverviewDto> GetOverviewAsync();
}
