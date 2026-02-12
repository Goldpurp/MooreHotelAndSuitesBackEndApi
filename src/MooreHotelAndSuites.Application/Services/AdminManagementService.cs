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
        private readonly ICurrentUserService _currentUser;
        private readonly IEmailService _emailService;
      private readonly IGuestRepository _guestRepo;
          public AdminManagementService(
            IUserManagementService users,
            ICurrentUserService currentUser,
            IBookingReadRepository bookingRepo,
            IGuestRepository guestRepo,
            IEmailService emailService)
        {
            _users = users;
            _currentUser = currentUser;
            _bookingRepo = bookingRepo;
            _guestRepo = guestRepo;
            _emailService = emailService;
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
            // Generate temporary password
            var tempPassword = "Tmp@" + Guid.NewGuid().ToString("N").Substring(0, 8);

              var adminId = _currentUser.UserId;

            if (string.IsNullOrEmpty(adminId))
                throw new UnauthorizedAccessException("Only authenticated admins can create staff");
                
           var createdUser = await _users.CreateUserAsync(
            dto.Email,
            dto.FullName,
            tempPassword,
            dto.Role,
            createdByAdminId: adminId // the ID of the admin performing the action
        );


            // Send temporary password via email
            await _emailService.SendAsync(
                to: dto.Email,
                subject: "Your Moore Hotel Staff Account",
                body: $@"Dear {dto.FullName},

        Your account at Moore Hotel & Suites has been created.

        Temporary login credentials:
        Username: {dto.Email}
        Password: {tempPassword}

        Please log in and change your password immediately.

        Regards,
        Moore Hotel Administration"
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
