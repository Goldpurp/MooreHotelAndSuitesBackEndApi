using MooreHotelAndSuites.Application.DTOs.Notifications;

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetMyAsync(string userId);
        Task<List<NotificationDto>> GetStaffAsync();
        Task MarkAsReadAsync(Guid id);
    }
}
