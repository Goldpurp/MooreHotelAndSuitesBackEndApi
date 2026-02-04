using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
   public interface IGuestRepository
{
    Task AddAsync(Guest guest);
    Task<Guest?> GetByIdAsync(int id);
    Task<List<Guest>> GetAllAsync();
    Task<int> CountAsync();
    Task<Guest?> FindByNameAsync(string fullName);
    Task<Guest?> FindByEmailAsync(string email);
    Task<Guest?> FindByPhoneAsync(string phone);
}

}
