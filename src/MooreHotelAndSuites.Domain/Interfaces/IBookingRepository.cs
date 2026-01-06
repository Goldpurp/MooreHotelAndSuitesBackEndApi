using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task AddAsync(Booking booking);
        Task<IEnumerable<Booking>> GetByRoomAsync(int roomId);
    }
}
