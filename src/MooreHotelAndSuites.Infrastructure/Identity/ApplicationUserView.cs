using MooreHotelAndSuites.Application.Interfaces.Identity;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
    internal sealed class ApplicationUserView : IApplicationUser
    {
        public ApplicationUserView(ApplicationUser user)
        {
            Id = user.Id;
            Email = user.Email!;
            FullName = user.FullName;
            IsActive = user.LockoutEnd == null;
            EmailConfirmed = user.EmailConfirmed;
        }

        public string Id { get; }
        public string Email { get; }
        public string? FullName { get; }
        public bool IsActive { get; }
        public bool EmailConfirmed { get; }
    }
}
