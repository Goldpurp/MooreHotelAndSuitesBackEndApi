using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Interfaces;
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
        // BASIC CRUD
        // =====================

        public async Task<Room?> GetByIdAsync(int roomId)
        {
            return await _db.Rooms
                .Include(r => r.RoomAmenities)
                .Include(r => r.Images)
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<IReadOnlyList<Room>> GetAllAsync()
        {
            return await _db.Rooms
                .Include(r => r.Images)
                .Include(r => r.RoomAmenities)
                .Include(r => r.Reviews)
                .ToListAsync();
        }

        public async Task AddAsync(Room room)
        {
            // Step 1: Add room to generate Id
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();

            // Step 2: Assign RoomId to nested entities
            foreach (var amenity in room.RoomAmenities)
            {
                amenity.RoomId = room.Id;
            }

            foreach (var image in room.Images)
            {
                image.RoomId = room.Id;
            }

            foreach (var review in room.Reviews)
            {
                review.RoomId = room.Id;
            }

            // Step 3: Save nested entities
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Room room)
        {
            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
        }

        // =====================
        // ADMIN OPERATIONS
        // =====================

        public async Task<Room?> GetRoomForAdminAsync(int roomId)
        {
            return await GetByIdAsync(roomId);
        }

        public async Task UpdateRoomAmenitiesAsync(int roomId, IReadOnlyCollection<int> amenityIds)
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
                    RoomId = roomId,
                    AmenityId = amenityId
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateRoomImagesAsync(int roomId, IReadOnlyCollection<RoomImage> images)
        {
            var room = await _db.Rooms
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null) return;

            room.Images.Clear();
            foreach (var img in images)
            {
                img.RoomId = roomId; // ensure foreign key is set
                room.Images.Add(img);
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateRoomStatusAsync(int roomId, RoomStatus status)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room == null) return;

            room.Status = status;
            room.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        // =====================
        // GUEST OPERATIONS
        // =====================

        public async Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int guests)
        {
            return await _db.Rooms
                .Include(r => r.Images)
                .Where(r =>
                    r.Status == RoomStatus.Available &&
                    r.Capacity >= guests)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomForGuestAsync(int roomId)
        {
            return await _db.Rooms
                .Include(r => r.Images)
                .Include(r => r.RoomAmenities)
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        // =====================
        // RATINGS
        // =====================

        public async Task UpdateRoomRatingAsync(int roomId)
        {
            var room = await _db.Rooms
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null || !room.Reviews.Any()) return;

            room.TotalReviews = room.Reviews.Count;
            room.AverageRating = room.Reviews.Average(r => r.Rating);

            await _db.SaveChangesAsync();
        }
    }
}
