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
         Task<RoomImage?> GetImageByIdAsync(Guid imageId);
        // WRITE
        Task AddAsync(Room room);
        Task AddRoomImageAsync(RoomImage image);
         Task ClearCoverImageAsync(Guid roomId);
        Task UpdateRoomAmenitiesAsync(
            Guid roomId,
            IReadOnlyCollection<Guid> amenityIds);

        Task UpdateRoomImagesAsync(
            Guid roomId,
            IReadOnlyCollection<RoomImage> images);

        Task UpdateRoomStatusAsync(
            Guid roomId,
            RoomStatus status);
        Task DeleteRoomImageAsync(RoomImage image);
        Task UpdateRoomRatingAsync(Guid roomId);
     

    }
}
