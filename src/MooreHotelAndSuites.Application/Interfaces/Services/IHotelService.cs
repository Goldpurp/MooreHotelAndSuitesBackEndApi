using MooreHotelAndSuites.Application.DTOs.Hotel;

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelDto>> GetAllAsync();
        Task<HotelDto?> GetAsync(int id);
        Task CreateAsync(CreateHotelDto dto);

    }
}