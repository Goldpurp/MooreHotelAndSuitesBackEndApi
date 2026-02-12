using MooreHotelAndSuites.Application.Interfaces.Identity;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
internal sealed class ApplicationUserView : IApplicationUser
{
    public ApplicationUserView(ApplicationUser user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        Id = user.Id ?? throw new ArgumentNullException(nameof(user.Id));
        UserName = user.UserName ?? throw new ArgumentNullException(nameof(user.UserName));
        Email = user.Email ?? throw new ArgumentNullException(nameof(user.Email));
        FullName = user.FullName; // nullable is fine
        EmailConfirmed = user.EmailConfirmed;
        CreatedByAdminId = user.CreatedByAdminId ?? throw new ArgumentNullException(nameof(user.CreatedByAdminId));
    }

    public string Id { get; }
    public string UserName { get; }
    public string Email { get; }
    public string? FullName { get; }
    public bool EmailConfirmed { get; }
    public string CreatedByAdminId { get; }
}

}
