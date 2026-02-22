using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MooreHotelAndSuites.Application.Interfaces.Identity;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? Email =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?
                .User?
                .Identity?
                .IsAuthenticated ?? false;

        public bool IsInRole(string role) =>
            _httpContextAccessor.HttpContext?
                .User?
                .IsInRole(role) ?? false;
    }
}
