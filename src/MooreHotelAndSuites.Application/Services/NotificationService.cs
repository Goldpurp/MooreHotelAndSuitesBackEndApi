using MooreHotelAndSuites.Application.DTOs.Notifications;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;


namespace MooreHotelAndSuites.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<NotificationDto>> GetMyAsync(string userId)
        {
            var notifications = await _repo.GetByUserAsync(userId);
            return notifications.Select(Map).ToList();
        }

        public async Task<List<NotificationDto>> GetStaffAsync()
        {
            var notifications = await _repo.GetStaffAsync();
            return notifications.Select(Map).ToList();
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            var notification = await _repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Notification not found");

            notification.IsRead = true;
            await _repo.SaveChangesAsync();
        }

        private static NotificationDto Map(Domain.Entities.Notification n) =>
            new(n.Id, n.Title, n.Message, n.IsRead, n.CreatedAt);
    }
}
