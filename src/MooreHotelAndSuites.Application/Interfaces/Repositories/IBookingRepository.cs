using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task AddAsync(Booking booking);
        Task<IEnumerable<Booking>> GetByRoomAsync(Guid roomId);
        Task UpdateAsync(Booking booking);
        Task SaveChangesAsync();
        Task DeleteAsync(Booking booking);
    }
}
