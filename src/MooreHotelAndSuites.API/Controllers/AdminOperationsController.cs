using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.API.Controllers;

[ApiController]
[Route("api/admin/operations")]
[Authorize(Policy = "AdminOnly")]
public sealed class AdminOperationsController : ControllerBase
{
    private readonly IOperationsService _ops;

    public AdminOperationsController(IOperationsService ops)
    {
        _ops = ops;
    }

    [HttpGet("ledger")]
    public async Task<IActionResult> Ledger()
        => Ok(await _ops.GetLedgerAsync());

    [HttpGet("daily-stats")]
    public async Task<IActionResult> DailyStats()
        => Ok(await _ops.GetDailyStatsAsync());
}
