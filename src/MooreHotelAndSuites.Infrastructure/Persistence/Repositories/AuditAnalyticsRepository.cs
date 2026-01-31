using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Application.DTOs.Analytics;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Infrastructure.Data;

namespace MooreHotelAndSuites.Infrastructure.Persistence.Repositories
{
    public class AuditAnalyticsRepository : IAuditAnalyticsRepository
    {
        private readonly AppDbContext _context;

        public AuditAnalyticsRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<int> CountByActionAsync(string action)
        {
            return _context.AuditLogs.CountAsync(a => a.Action == action);
        }

        public async Task<List<ActionCountDto>> ActionsPerDayAsync(DateTime from)
        {
            return await _context.AuditLogs
                .Where(a => a.OccurredAt >= from)
                .GroupBy(a => a.Action)
                .Select(g => new ActionCountDto
                {
                    Action = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
}
