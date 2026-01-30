using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Application.Interfaces.Identity;
using MooreHotelAndSuites.Infrastructure.Identity;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly RoleManager<IdentityRole> _roles;

        public UserManagementService(
            UserManager<ApplicationUser> users,
            RoleManager<IdentityRole> roles)
        {
            _users = users;
            _roles = roles;
        }

        public async Task<int> CountUsersAsync()
            => await _users.Users.CountAsync();

        public async Task<IReadOnlyList<IApplicationUser>> GetUsersInRoleAsync(string role)
        {
            var users = await _users.GetUsersInRoleAsync(role);
            return users.Select(u => new ApplicationUserView(u)).ToList();
        }

        public async Task CreateUserAsync(
            string email,
            string fullName,
            string password,
            string role)
        {
            if (!await _roles.RoleExistsAsync(role))
                throw new InvalidOperationException("Role does not exist");

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var result = await _users.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            await _users.AddToRoleAsync(user, role);
        }

        public async Task ActivateAsync(string userId)
        {
            var user = await _users.FindByIdAsync(userId)
                ?? throw new KeyNotFoundException();

            user.LockoutEnd = null;
            await _users.UpdateAsync(user);
        }

        public async Task DeactivateAsync(string userId)
        {
            var user = await _users.FindByIdAsync(userId)
                ?? throw new KeyNotFoundException();

            user.LockoutEnd = DateTimeOffset.MaxValue;
            await _users.UpdateAsync(user);
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await _users.FindByIdAsync(userId)
                ?? throw new KeyNotFoundException();

            await _users.DeleteAsync(user);
        }
    }
}
