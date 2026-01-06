using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.Domain.Interfaces
{
    public interface IRoomRepository
    {
        // =====================
        // BASIC CRUD
        // =====================
        Task<Room?> GetByIdAsync(int roomId);
        Task<IReadOnlyList<Room>> GetAllAsync();

        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(Room room);

        // =====================
        // ADMIN OPERATIONS
        // =====================
        Task<Room?> GetRoomForAdminAsync(int roomId);

        Task UpdateRoomAmenitiesAsync(
            int roomId,
            IReadOnlyCollection<int> amenityIds
        );

        Task UpdateRoomImagesAsync(
            int roomId,
            IReadOnlyCollection<RoomImage> images
        );

        Task UpdateRoomStatusAsync(
            int roomId,
            RoomStatus status
        );

        // =====================
        // GUEST OPERATIONS
        // =====================
        Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
            DateTime checkIn,
            DateTime checkOut,
            int guests
        );

        Task<Room?> GetRoomForGuestAsync(int roomId);

        // =====================
        // RATINGS
        // =====================
        Task UpdateRoomRatingAsync(int roomId);
    }
}
