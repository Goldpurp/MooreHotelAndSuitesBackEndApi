using Microsoft.AspNetCore.Identity;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Infrastructure.Identity;

namespace MooreHotelAndSuites.Infrastructure.Services
{
    public class IdentityEmailService : IIdentityLookupService
    {
        private readonly UserManager<ApplicationUser> _users;

        public IdentityEmailService(UserManager<ApplicationUser> users)
        {
            _users = users;
        }

        public async Task<string?> GetEmailByIdAsync(string identityId)
        {
            var user = await _users.FindByIdAsync(identityId);
            return user?.Email;
        }
    }
}
