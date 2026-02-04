using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task AddAsync(Booking booking);
        Task<IEnumerable<Booking>> GetByRoomAsync(Guid roomId);
        Task<Booking?> GetLastPendingAsync();
        Task<Booking?> GetRecentPendingByGuestAsync(    int guestId, TimeSpan window);
        Task<IEnumerable<Booking>> GetAllPendingAsync();
        Task<Booking?> GetLastPendingByGuestIdAsync(int guestId);
        Task UpdateAsync(Booking booking);
        Task SaveChangesAsync();
        Task DeleteAsync(Booking booking);
    }
}
