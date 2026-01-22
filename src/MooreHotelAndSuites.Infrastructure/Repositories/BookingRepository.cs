using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MooreHotelAndSuites.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _db;
        public BookingRepository(AppDbContext db) => _db = db;
        public async Task AddAsync(Booking booking) { await _db.Bookings.AddAsync(booking); await _db.SaveChangesAsync(); }
        public async Task<IEnumerable<Booking>> GetByRoomAsync(Guid roomId) => await _db.Bookings.Where(b=>b.RoomId==roomId).ToListAsync();
        public async Task<Booking?> GetByIdAsync(Guid id) => await _db.Bookings.FindAsync(id);
        public async Task DeleteAsync(Booking booking)
        {
            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();
        }

    }
}
