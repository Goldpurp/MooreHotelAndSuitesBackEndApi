using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Infrastructure.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _db;
        public BookingRepository(AppDbContext db) => _db = db;
        public async Task AddAsync(Booking booking) { await _db.Bookings.AddAsync(booking); await _db.SaveChangesAsync(); }
        public async Task<IEnumerable<Booking>> GetByRoomAsync(Guid roomId) => await _db.Bookings.Where(b=>b.RoomId==roomId).ToListAsync();
        public async Task<Booking?> GetByIdAsync(Guid id) => await _db.Bookings.FindAsync(id);
        
        public async Task UpdateAsync(Booking booking)
        {
            _db.Bookings.Update(booking);
            await Task.CompletedTask;
        }
        public async Task<Booking?> GetLastPendingAsync()
        {
            return await _db.Bookings
                .Where(b => b.Status == BookingStatus.Pending)
                .OrderBy(b => b.CheckIn)
                .FirstOrDefaultAsync();
        }
        public async Task<Booking?> GetRecentPendingByGuestAsync(
    int guestId,
    TimeSpan window)
{
    var cutoff = DateTime.UtcNow.Subtract(window);

    return await _db.Bookings
        .Where(b =>
            b.GuestId == guestId &&
            b.Status == BookingStatus.Pending &&
            b.CreatedAt >= cutoff)
        .FirstOrDefaultAsync();
}

public async Task<Booking?> GetLastPendingByGuestIdAsync(int guestId)
{
    return await _db.Bookings
        .Where(b =>
            b.Status == BookingStatus.Pending &&
            b.GuestId == guestId)
        .OrderByDescending(b => b.CreatedAt)
        .FirstOrDefaultAsync();
}


public async Task<IEnumerable<Booking>> GetAllPendingAsync()
{
    return await _db.Bookings
        .Where(b => b.Status == BookingStatus.Pending)
        .OrderByDescending(b => b.CreatedAt)
        .ToListAsync();
}

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(Booking booking)
        {
            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();
        }

    }
}
