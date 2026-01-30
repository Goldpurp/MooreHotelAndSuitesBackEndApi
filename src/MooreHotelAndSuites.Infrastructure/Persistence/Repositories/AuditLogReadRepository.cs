using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Application.DTOs.Ledger;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Infrastructure.Data;
using MooreHotelAndSuites.Application.DTOs.Analytics;
namespace MooreHotelAndSuites.Infrastructure.Persistence.Repositories;

public sealed class AuditLogReadRepository : IAuditLogReadRepository
{
    private readonly AppDbContext _context;

    public AuditLogReadRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LedgerEntryDto>> GetLedgerAsync()
    {
        return await _context.AuditLogs
            .OrderByDescending(a => a.OccurredAt)
            .Select(a => new LedgerEntryDto
            {
                Timestamp = a.OccurredAt,
                ActorUserId = a.UserId,
                Action = a.Action,
                Resource = a.Path
            })
            .ToListAsync();
    }

    public Task<int> CountByActionAsync(string action)
    {
        return _context.AuditLogs.CountAsync(a => a.Action.Contains(action));
    }

    public async Task<List<ActionCountDto>> CountActionsPerDayAsync(DateTime from)
    {
        return await _context.AuditLogs
            .Where(a => a.OccurredAt >= from)
            .GroupBy(a => a.OccurredAt.Date)
          .Select(g => new ActionCountDto
        {
            Date = g.Key,
            Count = g.Count()
        })


            .ToListAsync();
    }
}
