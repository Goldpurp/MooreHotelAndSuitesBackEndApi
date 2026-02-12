namespace MooreHotelAndSuites.Application.Interfaces.Identity
{
    public interface IApplicationUser
{
    string Id { get; }
    string? UserName { get; }   
    string? Email { get; }
    string? FullName { get; }
    bool EmailConfirmed { get; }
    string? CreatedByAdminId { get; } 
}

}
