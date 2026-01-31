using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Application.DTOs.Analytics;
namespace MooreHotelAndSuites.API.Controllers.Admin;

[ApiController]
[Route("api/admin/analytics")]
[Authorize(Policy = "AdminOnly")]
public class AdminAnalyticsController : ControllerBase
{
    private readonly IAuditAnalyticsService _analytics;

    public AdminAnalyticsController(IAuditAnalyticsService analytics)
    {
        _analytics = analytics;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview(
        [FromQuery] int days = 30)
    {
        var from = DateTime.UtcNow.AddDays(-days);

       var overview = new AnalyticsOverviewDto
{
    TotalAdminActions =
        await _analytics.CountActionsAsync("ADMIN"),

    TotalLogins =
        await _analytics.CountActionsAsync("POST /api/auth/login"),

    ActionsLast7Days =
        await _analytics.ActionsPerDayAsync(from)
};

        return Ok(overview);

        
    }
}
