using MooreHotelAndSuites.Application.DTOs.Guests;

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IGuestService
    {
        Task<GuestDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateGuestDto dto);
    }
}
