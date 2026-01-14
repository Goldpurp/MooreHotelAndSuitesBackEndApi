using MooreHotelAndSuites.Application.DTOs.Hotel;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _repo;

        public HotelService(IHotelRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<HotelDto>> GetAllAsync()
        {
            var hotels = await _repo.GetAllAsync();

            return hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Location = h.Location
            });
        }

        public async Task<HotelDto?> GetAsync(int id)
        {
            var hotel = await _repo.GetByIdAsync(id);
            if (hotel == null) return null;

            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Location = hotel.Location
            };
        }

        public async Task CreateAsync(CreateHotelDto dto)
        {
            var hotel = new Hotel
            {
                Name = dto.Name,
                Location = dto.Location
            };

            await _repo.AddAsync(hotel);
        }
    }
}
