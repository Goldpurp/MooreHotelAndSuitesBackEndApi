using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Domain.Interfaces
{
    public interface IGuestRepository
    {
        Task<Guest?> GetByIdAsync(int id);
        Task AddAsync(Guest guest);
    }
}
