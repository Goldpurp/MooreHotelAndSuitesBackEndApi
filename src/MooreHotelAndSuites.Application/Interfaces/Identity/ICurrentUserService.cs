using System.Security.Claims;

namespace MooreHotelAndSuites.Application.Interfaces.Identity
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}
