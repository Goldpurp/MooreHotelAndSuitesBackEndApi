using MooreHotelAndSuites.Application.DTOs.Rooms;

namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IRoomCommandService
    {
        Task<Guid> CreateAsync(CreateRoomDto dto);

        Task AddImageAsync(Guid roomId, CreateRoomImageDto image);

        Task UpdateAmenitiesAsync(
            Guid roomId,
            IReadOnlyCollection<Guid> amenityIds);

        Task UpdateImagesAsync(
            Guid roomId,
            IReadOnlyCollection<CreateRoomImageDto> images);

        Task UpdateStatusAsync(UpdateRoomStatusDto dto);

        Task UpdateRatingAsync(Guid roomId);
       Task DeleteImageAsync(Guid imageId);
}
}