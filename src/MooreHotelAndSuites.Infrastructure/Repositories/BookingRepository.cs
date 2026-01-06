using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Interfaces;
using MooreHotelAndSuites.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MooreHotelAndSuites.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _db;
        public BookingRepository(AppDbContext db) => _db = db;
        public async Task AddAsync(Booking booking) { await _db.Bookings.AddAsync(booking); await _db.SaveChangesAsync(); }
        public async Task<IEnumerable<Booking>> GetByRoomAsync(int roomId) => await _db.Bookings.Where(b=>b.RoomId==roomId).ToListAsync();
        public async Task<Booking?> GetByIdAsync(int id) => await _db.Bookings.FindAsync(id);
    }
}
