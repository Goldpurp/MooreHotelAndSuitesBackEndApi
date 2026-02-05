

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IIdentityLookupService
{
    Task<string?> GetEmailByIdAsync(string identityId);
}

}