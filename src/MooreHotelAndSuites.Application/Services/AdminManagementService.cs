using MooreHotelAndSuites.Application.DTOs.Admin;
using MooreHotelAndSuites.Application.DTOs.Guests;
using MooreHotelAndSuites.Application.Interfaces.Identity;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.Application.Services
{
    public class AdminManagementService : IAdminManagementService
    {
        private readonly IUserManagementService _users;
        private readonly IBookingReadRepository _bookingRepo;
      private readonly IGuestRepository _guestRepo;
        public AdminManagementService(
            IUserManagementService users,
            IBookingReadRepository bookingRepo,
            IGuestRepository guestRepo)
        {
            _users = users;
            _bookingRepo = bookingRepo;
            _guestRepo =  guestRepo;
        }

        public async Task<AdminStatsDto> GetStatsAsync()
        {
            var totalUsers = await _users.CountUsersAsync();

            var receptionists = await _users.GetUsersInRoleAsync("Receptionist");
            var managers = await _users.GetUsersInRoleAsync("Manager");
            var admins = await _users.GetUsersInRoleAsync("Admin");

            var guests = await _guestRepo.CountAsync();   // better than role-based guests
            var activeBookings = await _bookingRepo.CountActiveBookingsAsync();

            return new AdminStatsDto(
                totalUsers,
                receptionists.Count + managers.Count,   // employees
                guests,
                activeBookings
            );
        }


       public async Task<IReadOnlyList<IApplicationUser>> GetEmployeesAsync()
        {
            var receptionists = await _users.GetUsersInRoleAsync("Receptionist");
            var managers = await _users.GetUsersInRoleAsync("Manager");

            return receptionists
                .Concat(managers)
                .DistinctBy(u => u.Id)
                .ToList();
        }


    public async Task<IReadOnlyList<GuestDto>> GetGuestsAsync()
    {
        var guests = await _guestRepo.GetAllAsync();
        return guests.Select(g => new GuestDto
        {
            Id = g.Id,
            FullName = g.FullName,
            Email = g.Email,
            PhoneNumber = g.PhoneNumber
        }).ToList();
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
