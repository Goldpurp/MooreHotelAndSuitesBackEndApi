using MooreHotelAndSuites.Application.DTOs.Notifications;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Application.Interfaces.Realtime;

namespace MooreHotelAndSuites.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IRealtimeNotifier _realtime;

        public NotificationService(
            INotificationRepository repo,
            IRealtimeNotifier realtime)
        {
            _repo = repo;
            _realtime = realtime;   // ✅ FIXED
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

        public async Task BroadcastAsync(string channel, object payload)
        {
            // ✅ use abstraction instead of SignalR directly
            await _realtime.BroadcastAsync(channel, payload);
        }

        public async Task NotifyStaffAsync(string title, string message)
        {
            var notification =
                Domain.Entities.Notification.CreateStaff(title, message);

            await _repo.AddAsync(notification);
            await _repo.SaveChangesAsync();

            await BroadcastAsync("staff", new
            {
                type = "STAFF_NOTIFICATION",
                title,
                message,
                id = notification.Id
            });
        }

        private static NotificationDto Map(
            Domain.Entities.Notification n) =>
            new(n.Id, n.Title, n.Message, n.IsRead, n.CreatedAt);
    }
}
