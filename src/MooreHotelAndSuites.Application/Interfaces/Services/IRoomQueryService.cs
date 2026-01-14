using MooreHotelAndSuites.Application.DTOs.Rooms;


namespace MooreHotelAndSuites.Application.Interfaces.Services
{
    public interface IRoomQueryService
    {
        Task<IReadOnlyList<RoomListDto>> GetAllAsync();

        Task<RoomDetailsDto?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<RoomListDto>> GetAvailableAsync(
            DateTime checkIn,
            DateTime checkOut,
            int guests);
    }
}
