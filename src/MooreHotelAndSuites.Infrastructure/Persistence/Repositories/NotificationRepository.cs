using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Infrastructure.Data;

namespace MooreHotelAndSuites.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Notification>> GetByUserAsync(string userId) =>
            _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        public Task<List<Notification>> GetStaffAsync() =>
            _context.Notifications
                .Where(n => n.UserId == "STAFF")
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        public Task<Notification?> GetByIdAsync(Guid id) =>
            _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
