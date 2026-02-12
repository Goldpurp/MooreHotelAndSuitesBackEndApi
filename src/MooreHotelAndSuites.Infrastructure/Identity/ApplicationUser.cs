using Microsoft.AspNetCore.Identity;
using MooreHotelAndSuites.Application.Interfaces.Identity;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser, IApplicationUser
{
    public string? FullName { get; set; }
      public bool MustChangePassword { get; set; } = true;
    public string? CreatedByAdminId { get; set; }
    public DateTime CreatedOn { get; set; }
  
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}

}
