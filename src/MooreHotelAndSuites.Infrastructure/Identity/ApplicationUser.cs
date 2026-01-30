using Microsoft.AspNetCore.Identity;
using MooreHotelAndSuites.Application.Interfaces.Identity;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser, IApplicationUser
{
    public string? FullName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}

}
