using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Infrastructure.Data;

namespace MooreHotelAndSuites.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _db;

        public RoomRepository(AppDbContext db)
        {
            _db = db;
        }

        // =====================
        // READ OPERATIONS
        // =====================

        public async Task<Room?> GetByIdAsync(Guid roomId)
        {
            return await _db.Rooms
                .Include(r => r.Images)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<IReadOnlyList<Room>> GetAllAsync()
        {
            return await _db.Rooms
                .Include(r => r.Images)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
            DateTime checkIn,
            DateTime checkOut,
            int guests)
        {
            return await _db.Rooms
                .Include(r => r.Images)
                .Where(r =>
                    r.Status == RoomStatus.Available &&
                    r.Capacity >= guests)
                .ToListAsync();
        }

        // =====================
        // WRITE OPERATIONS
        // =====================

        public async Task AddAsync(Room room)
        {
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRoomAmenitiesAsync(
            Guid roomId,
            IReadOnlyCollection<Guid> amenityIds)
        {
            var room = await _db.Rooms
                .Include(r => r.RoomAmenities)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null) return;

            room.RoomAmenities.Clear();

            foreach (var amenityId in amenityIds)
            {
                room.RoomAmenities.Add(new RoomAmenity
                {
                    RoomId = room.Id,
                    AmenityId = amenityId
                });
            }

            room.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRoomImagesAsync(
            Guid roomId,
            IReadOnlyCollection<RoomImage> images)
        {
            var room = await _db.Rooms
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null) return;

            room.Images.Clear();

            foreach (var image in images)
            {
                image.RoomId = roomId;
                room.Images.Add(image);
            }

            room.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRoomStatusAsync(
            Guid roomId,
            RoomStatus status)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room == null) return;

            room.Status = status;
            room.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task UpdateRoomRatingAsync(Guid roomId)
        {
            var room = await _db.Rooms
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null || !room.Reviews.Any())
                return;

            room.TotalReviews = room.Reviews.Count;
            room.AverageRating = room.Reviews.Average(r => r.Rating);
            room.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}
