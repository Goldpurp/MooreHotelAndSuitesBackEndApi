using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
    public interface IGuestRepository
    {
        Task<Guest?> GetByIdAsync(int id);
        Task AddAsync(Guest guest);
    }
}
