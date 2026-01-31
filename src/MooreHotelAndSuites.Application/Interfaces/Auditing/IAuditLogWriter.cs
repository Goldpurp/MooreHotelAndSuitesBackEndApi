using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Interfaces.Auditing
{
public interface IAuditLogWriter
{
    Task WriteAsync(string userId, string action, string path);
}


}