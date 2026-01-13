using MooreHotelAndSuites.Application.DTOs.Rooms;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Application.Services
{
    public class RoomCommandService : IRoomCommandService
    {
        private readonly IRoomRepository _repo;

        public RoomCommandService(IRoomRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> CreateAsync(CreateRoomDto dto)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                RoomNumber = dto.RoomNumber,
                RoomName = dto.RoomName,
                Description = dto.Description,
                PricePerNight = dto.PricePerNight,
                Capacity = dto.Capacity,
                Size = dto.Size,
                RoomType = (RoomType)dto.RoomType,
                Status = RoomStatus.Available,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(room);

            if (dto.AmenityIds.Any())
                await _repo.UpdateRoomAmenitiesAsync(room.Id, dto.AmenityIds);

            return room.Id;
        }

        public Task UpdateAmenitiesAsync(
            Guid roomId,
            IReadOnlyCollection<Guid> amenityIds)
        {
            return _repo.UpdateRoomAmenitiesAsync(roomId, amenityIds);
        }

        public Task UpdateImagesAsync(
            Guid roomId,
            IReadOnlyCollection<CreateRoomImageDto> images)
        {
            var roomImages = images
                .Select(i => new RoomImage
                {
                    Id = Guid.NewGuid(),
                    RoomId = roomId,
                    ImageUrl = i.ImageUrl,
                    IsCover = i.IsCover,
                    DisplayOrder = i.DisplayOrder
                })
                .ToList()
                .AsReadOnly();

            return _repo.UpdateRoomImagesAsync(roomId, roomImages);
        }

        public Task UpdateStatusAsync(UpdateRoomStatusDto dto)
        {
            return _repo.UpdateRoomStatusAsync(
                dto.RoomId,
                (RoomStatus)dto.Status);
        }

        public Task UpdateRatingAsync(Guid roomId)
        {
            return _repo.UpdateRoomRatingAsync(roomId);
        }
    }
}
