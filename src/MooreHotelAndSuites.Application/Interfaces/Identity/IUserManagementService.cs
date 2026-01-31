using MooreHotelAndSuites.Application.Interfaces.Identity;
public interface IUserManagementService
{
    Task<int> CountUsersAsync();
    Task<IReadOnlyList<IApplicationUser>> GetUsersInRoleAsync(string role);
    Task CreateUserAsync(string email, string fullName, string password, string role);
    Task ActivateAsync(string userId);
    Task DeactivateAsync(string userId);
    Task DeleteAsync(string userId);
}
