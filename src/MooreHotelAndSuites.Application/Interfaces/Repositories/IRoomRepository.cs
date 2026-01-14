using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        // READ
        Task<Room?> GetByIdAsync(Guid roomId);
        Task<IReadOnlyList<Room>> GetAllAsync();
        Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
            DateTime checkIn,
            DateTime checkOut,
            int guests);

        // WRITE
        Task AddAsync(Room room);
        Task UpdateRoomAmenitiesAsync(
            Guid roomId,
            IReadOnlyCollection<Guid> amenityIds);

        Task UpdateRoomImagesAsync(
            Guid roomId,
            IReadOnlyCollection<RoomImage> images);

        Task UpdateRoomStatusAsync(
            Guid roomId,
            RoomStatus status);

        Task UpdateRoomRatingAsync(Guid roomId);
    }
}
