using System.Security.Claims;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Infrastructure.Data;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;

    public AuditLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        await _next(context);

        if (context.Request.Path.StartsWithSegments("/api"))
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var log = new AuditLog
            {
                UserId = userId,
                Action = context.GetEndpoint()?.DisplayName ?? "Unknown",
                Path = context.Request.Path,
                Method = context.Request.Method,
                StatusCode = context.Response.StatusCode
            };

            db.AuditLogs.Add(log);
            await db.SaveChangesAsync();
        }
    }
}
