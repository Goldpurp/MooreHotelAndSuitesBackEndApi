using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Interfaces;
using MooreHotelAndSuites.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MooreHotelAndSuites.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _db;
        public HotelRepository(AppDbContext db) => _db = db;
        public async Task AddAsync(Hotel hotel) { await _db.Hotels.AddAsync(hotel); await _db.SaveChangesAsync(); }
        public async Task<IEnumerable<Hotel>> GetAllAsync() => await _db.Hotels.Include(h=>h.Rooms).ToListAsync();
        public async Task<Hotel?> GetByIdAsync(int id) => await _db.Hotels.Include(h=>h.Rooms).FirstOrDefaultAsync(h=>h.Id==id);
    }
}
