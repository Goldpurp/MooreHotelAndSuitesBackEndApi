using MooreHotelAndSuites.Application.Interfaces.Identity;

namespace MooreHotelAndSuites.Application.Interfaces.Identity
{
    public interface IUserManagementService
    {
        Task<int> CountUsersAsync();
        Task<IReadOnlyList<IApplicationUser>> GetUsersInRoleAsync(string role);

   
     Task<IApplicationUser> CreateUserAsync(
    string email,
    string fullName,
    string password,
    string role,
    string? createdByAdminId = null
);

        Task ActivateAsync(string userId);
        Task DeactivateAsync(string userId);
        Task DeleteAsync(string userId);
    }
}
