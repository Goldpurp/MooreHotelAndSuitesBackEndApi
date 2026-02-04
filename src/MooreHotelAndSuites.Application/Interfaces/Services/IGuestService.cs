using MooreHotelAndSuites.Application.DTOs.Guests;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
  public interface IGuestService
{
    Task<int> EnsureGuestAsync(
        string fullName,
        string email,
        string phone);

    Task<Guest?> GetByIdAsync(int id);
     Task<Guest?> FindByNameAsync(string fullName);

    Task<Guest?> FindByPhoneAsync(string phone);
}

}
