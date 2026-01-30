using MooreHotelAndSuites.Infrastructure.Data;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Application.Interfaces.Auditing;


namespace MooreHotelAndSuites.Infrastructure.Auditing
{
    public class AuditLogWriter : IAuditLogWriter
{
    private readonly AppDbContext _context;

    public AuditLogWriter(AppDbContext context)
    {
        _context = context;
    }

    public async Task WriteAsync(string userId, string action, string path)
    {
        _context.AuditLogs.Add(new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Action = action,
            Path = path,
            OccurredAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }
}

}
