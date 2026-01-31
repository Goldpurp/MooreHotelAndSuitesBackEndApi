namespace MooreHotelAndSuites.Application.DTOs.Ledger
{
    public sealed class LedgerEntryDto
    {
        public DateTime Timestamp { get; init; }
        public string ActorUserId { get; init; } = string.Empty;
        public string Action { get; init; } = string.Empty;
        public string Resource { get; init; } = string.Empty;
    } 
};


