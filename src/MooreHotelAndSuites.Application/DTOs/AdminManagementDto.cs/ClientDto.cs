namespace MooreHotelAndSuites.Application.DTOs.Admin;

public sealed class ClientDto
{
    public string Id { get; init; } = default!;
    public string Email { get; init; } = default!;
    public bool IsActive { get; init; }
}
