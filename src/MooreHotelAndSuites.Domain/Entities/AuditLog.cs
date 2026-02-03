namespace MooreHotelAndSuites.Domain.Entities
{
    public class AuditLog
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }

    public string Action { get; set; } = default!;
    public string Entity { get; set; } = default!;
    public string Method { get; set; } = default!;
    public string Path { get; set; } = default!;
    public int StatusCode { get; set; } = default!;
    public DateTime OccurredAt { get; set; }
}

}
