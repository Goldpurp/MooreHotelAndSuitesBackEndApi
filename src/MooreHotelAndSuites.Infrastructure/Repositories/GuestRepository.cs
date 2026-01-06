using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Interfaces;
using MooreHotelAndSuites.Infrastructure.Data;

namespace MooreHotelAndSuites.Infrastructure.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly AppDbContext _db;
        public GuestRepository(AppDbContext db) => _db = db;
        public async Task AddAsync(Guest guest) { await _db.Guests.AddAsync(guest); await _db.SaveChangesAsync(); }
        public async Task<Guest?> GetByIdAsync(int id) => await _db.Guests.FindAsync(id);
    }
}
