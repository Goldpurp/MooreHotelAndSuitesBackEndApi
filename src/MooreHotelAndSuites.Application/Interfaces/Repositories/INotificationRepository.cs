using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetByUserAsync(string userId);
        Task<List<Notification>> GetStaffAsync();
        Task<Notification?> GetByIdAsync(Guid id);
        Task AddAsync(Notification notification);
        Task SaveChangesAsync();
    }
}
