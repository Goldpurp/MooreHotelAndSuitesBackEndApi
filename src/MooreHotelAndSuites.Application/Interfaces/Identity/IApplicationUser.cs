namespace MooreHotelAndSuites.Application.Interfaces.Identity
{
    public interface IApplicationUser
    {
        string Id { get; }
        string? Email { get; } 
        string? FullName { get; }
        bool EmailConfirmed { get; }
    }
}
