namespace MooreHotelAndSuites.Application.DTOs.Admin;

public sealed class EmployeeDto
{
    public string Id { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string? FullName { get; init; }
    public string Role { get; init; } = default!;
    public bool IsActive { get; init; }
}
