using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Interfaces;
namespace MooreHotelAndSuites.Application.Services
{
    public class GuestService
    {
        private readonly IGuestRepository _repo;
        public GuestService(IGuestRepository repo) => _repo = repo;
        public Task<Guest?> GetAsync(int id) => _repo.GetByIdAsync(id);
        public Task CreateAsync(Guest g) => _repo.AddAsync(g);
    }
}
