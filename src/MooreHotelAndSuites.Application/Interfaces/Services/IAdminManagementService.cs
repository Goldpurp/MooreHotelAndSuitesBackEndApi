using MooreHotelAndSuites.Application.DTOs.Admin;
using MooreHotelAndSuites.Application.Interfaces.Identity;
namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IAdminManagementService
    {
        Task<AdminStatsDto> GetStatsAsync();
         Task<IReadOnlyList<IApplicationUser>> GetEmployeesAsync();
        Task<IReadOnlyList<IApplicationUser>> GetClientsAsync();
        Task OnboardStaffAsync(OnboardStaffDto dto);
        Task ActivateAccountAsync(string userId);
        Task DeactivateAccountAsync(string userId);
        Task DeleteAccountAsync(string userId);
    }
}
