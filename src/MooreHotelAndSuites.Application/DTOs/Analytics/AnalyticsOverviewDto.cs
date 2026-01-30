namespace MooreHotelAndSuites.Application.DTOs.Analytics
{
    public sealed class AnalyticsOverviewDto
    {
        public int TotalAdminActions { get; init; }
        public int TotalLogins { get; init; }
        public IReadOnlyList<ActionCountDto> ActionsLast7Days { get; init; }
            = Array.Empty<ActionCountDto>();
    }
}
