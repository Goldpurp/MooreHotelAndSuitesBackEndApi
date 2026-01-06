using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Domain.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetByIdAsync(int id);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task AddAsync(Hotel hotel);
    }
}
