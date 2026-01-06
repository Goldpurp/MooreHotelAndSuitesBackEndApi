using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Interfaces;

namespace MooreHotelAndSuites.Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _repo;

        public RoomService(IRoomRepository repo)
        {
            _repo = repo;
        }

        // =====================
        // BASIC OPERATIONS
        // =====================
        public Task<IReadOnlyList<Room>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Room?> GetAsync(int id) => _repo.GetByIdAsync(id);

        public Task CreateAsync(Room room) => _repo.AddAsync(room);

        // =====================
        // OPTIONAL ADMIN OPERATIONS
        // =====================
        public Task<Room?> GetForAdminAsync(int id) => _repo.GetRoomForAdminAsync(id);

        public Task UpdateAmenitiesAsync(int roomId, IReadOnlyCollection<int> amenityIds) =>
            _repo.UpdateRoomAmenitiesAsync(roomId, amenityIds);

        public Task UpdateImagesAsync(int roomId, IReadOnlyCollection<RoomImage> images) =>
            _repo.UpdateRoomImagesAsync(roomId, images);

        public Task UpdateStatusAsync(int roomId, RoomStatus status) =>
            _repo.UpdateRoomStatusAsync(roomId, status);

        // =====================
        // GUEST OPERATIONS
        // =====================
        public Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int guests) =>
            _repo.GetAvailableRoomsAsync(checkIn, checkOut, guests);

        public Task<Room?> GetForGuestAsync(int roomId) => _repo.GetRoomForGuestAsync(roomId);

        // =====================
        // RATINGS
        // =====================
        public Task UpdateRatingAsync(int roomId) => _repo.UpdateRoomRatingAsync(roomId);
    }
}
