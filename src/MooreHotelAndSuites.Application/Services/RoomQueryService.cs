using MooreHotelAndSuites.Application.DTOs.Rooms;
using MooreHotelAndSuites.Application.Interfaces.Repositories;



namespace MooreHotelAndSuites.Application.Services
{
    public class RoomQueryService
    {
        private readonly IRoomRepository _repo;

        public RoomQueryService(IRoomRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<RoomListDto>> GetAllAsync()
        {
            var rooms = await _repo.GetAllAsync();

            return rooms.Select(r => new RoomListDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                RoomName = r.RoomName,
                PricePerNight = r.PricePerNight,
                Capacity = r.Capacity,
                AverageRating = r.AverageRating,
                CoverImageUrl = r.Images
                    .FirstOrDefault(i => i.IsCover)?.ImageUrl ?? string.Empty
            }).ToList();
        }

      public async Task<RoomDetailsDto?> GetByIdAsync(Guid id)
{
    var room = await _repo.GetByIdAsync(id);
    if (room == null) return null;

    return new RoomDetailsDto
    {
        Id = room.Id,
        RoomNumber = room.RoomNumber,
        RoomName = room.RoomName,
        Description = room.Description,
        PricePerNight = room.PricePerNight,
        Capacity = room.Capacity,
        Size = room.Size,
        RoomType = room.RoomType.ToString(),
        Status = room.Status.ToString(),
        AverageRating = room.AverageRating,
        TotalReviews = room.TotalReviews,
        Amenities = room.RoomAmenities.Select(a => a.Amenity!.Name),
        Images = room.Images
            .OrderBy(i => i.DisplayOrder)
            .Select(i => i.ImageUrl)
    };
}


        public async Task<IReadOnlyList<RoomListDto>> GetAvailableAsync(
            DateTime checkIn,
            DateTime checkOut,
            int guests)
        {
            var rooms = await _repo.GetAvailableRoomsAsync(checkIn, checkOut, guests);

            return rooms.Select(r => new RoomListDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                RoomName = r.RoomName,
                PricePerNight = r.PricePerNight,
                Capacity = r.Capacity,
                AverageRating = r.AverageRating,
                CoverImageUrl = r.Images
                    .FirstOrDefault(i => i.IsCover)?.ImageUrl ?? string.Empty
            }).ToList();
        }
    }
}
