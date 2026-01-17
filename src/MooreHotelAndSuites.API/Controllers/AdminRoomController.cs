using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.DTOs.Rooms;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/admin/rooms")]
    [Authorize(Roles = "Admin")]
    public class AdminRoomsController : ControllerBase
    {
        private readonly IRoomCommandService _commandService;

        public AdminRoomsController(IRoomCommandService commandService)
        {
            _commandService = commandService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
        {
            var roomId = await _commandService.CreateAsync(dto);

            return CreatedAtRoute(
                routeName: "GetRoomById",
                routeValues: new { id = roomId },
                value: new { RoomId = roomId }
            );
        }

   
        [HttpPut("{roomId:guid}/amenities")]
        public async Task<IActionResult> UpdateAmenities(
            Guid roomId,
            [FromBody] IReadOnlyCollection<Guid> amenityIds)
        {
            await _commandService.UpdateAmenitiesAsync(roomId, amenityIds);
            return NoContent();
        }

        
        [HttpPut("{roomId:guid}/images")]
        public async Task<IActionResult> UpdateImages(
            Guid roomId,
            [FromBody] IReadOnlyCollection<CreateRoomImageDto> images)
        {
            await _commandService.UpdateImagesAsync(roomId, images);
            return NoContent();
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(
            [FromBody] UpdateRoomStatusDto dto)
        {
            await _commandService.UpdateStatusAsync(dto);
            return NoContent();
        }
    }
}
