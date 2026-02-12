namespace MooreHotelAndSuites.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public static Notification CreateStaff(string title, string message)
        {
            return new Notification
            {
                Id = Guid.NewGuid(),
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,

                // You DON'T have this property → remove it
                // RecipientType = "STAFF"
            };
        }
    }
}
