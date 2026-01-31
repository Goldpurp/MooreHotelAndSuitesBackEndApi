using MooreHotelAndSuites.Application.DTOs.Admin;
using MooreHotelAndSuites.Application.Interfaces.Identity;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.Application.Services
{
    public class AdminManagementService : IAdminManagementService
    {
        private readonly IUserManagementService _users;
        private readonly IBookingReadRepository _bookingRepo;

        public AdminManagementService(
            IUserManagementService users,
            IBookingReadRepository bookingRepo)
        {
            _users = users;
            _bookingRepo = bookingRepo;
        }

        public async Task<AdminStatsDto> GetStatsAsync()
        {
            var totalUsers = await _users.CountUsersAsync();
            var staff = await _users.GetUsersInRoleAsync("Staff");
            var guests = await _users.GetUsersInRoleAsync("Guest");
            var activeBookings = await _bookingRepo.CountActiveBookingsAsync();

            return new AdminStatsDto(
                totalUsers,
                staff.Count,
                guests.Count,
                activeBookings
            );
        }

        public async Task<IReadOnlyList<IApplicationUser>> GetEmployeesAsync()
        {
            var staff = await _users.GetUsersInRoleAsync("Staff");
            var admins = await _users.GetUsersInRoleAsync("Admin");

            return staff.Concat(admins).DistinctBy(u => u.Id).ToList();
        }

        public async Task<IReadOnlyList<IApplicationUser>> GetClientsAsync()
        {
            return await _users.GetUsersInRoleAsync("Guest");
        }

        public async Task OnboardStaffAsync(OnboardStaffDto dto)
        {
            
            await _users.CreateUserAsync(
                dto.Email,
                dto.FullName,
                dto.Password,
                dto.Role
            );
        }

        public Task ActivateAccountAsync(string userId)
            => _users.ActivateAsync(userId);

        public Task DeactivateAccountAsync(string userId)
            => _users.DeactivateAsync(userId);

        public Task DeleteAccountAsync(string userId)
            => _users.DeleteAsync(userId);
    }
}
