using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Infrastructure.Data;
using MooreHotelAndSuites.Application.DTOs.Ledger;

[ApiController]
[Route("api/audit-logs")]
[Authorize(Roles = "Admin")]
public class AuditLogsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuditLogsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<LedgerEntryDto>>> Get()
    {
        var logs = await _context.AuditLogs
            .OrderByDescending(a => a.OccurredAt)
            .Select(a => new LedgerEntryDto
            {
                Timestamp = a.OccurredAt,
                ActorUserId = a.UserId,
                Action = a.Action,
                Resource = a.Path
            })
            .ToListAsync();

        return Ok(logs);
    }
}
