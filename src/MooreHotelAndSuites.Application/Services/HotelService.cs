using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Interfaces;
namespace MooreHotelAndSuites.Application.Services
{
    public class HotelService
    {
        private readonly IHotelRepository _repo;
        public HotelService(IHotelRepository repo) => _repo = repo;
        public Task<IEnumerable<Hotel>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Hotel?> GetAsync(int id) => _repo.GetByIdAsync(id);
        public Task CreateAsync(Hotel h) => _repo.AddAsync(h);
    }
}
