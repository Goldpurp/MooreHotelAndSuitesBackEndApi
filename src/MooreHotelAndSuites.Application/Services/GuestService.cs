using MooreHotelAndSuites.Application.DTOs.Guests;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Application.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _repo;

        public GuestService(IGuestRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateAsync(CreateGuestDto dto)
        {
            var guest = new Guest
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            await _repo.AddAsync(guest);
            return guest.Id;
        }

        public async Task<GuestDto?> GetByIdAsync(int id)
        {
            var guest = await _repo.GetByIdAsync(id);
            if (guest == null) return null;

            return new GuestDto
            {
                Id = guest.Id,
                FullName = guest.FullName,
                Email = guest.Email,
                PhoneNumber = guest.PhoneNumber
            };
        }
    }
}
