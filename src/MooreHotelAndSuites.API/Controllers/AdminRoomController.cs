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
        private readonly IImageStorageService _imageStorage;
          private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        private static readonly string[] AllowedTypes =
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };
        public AdminRoomsController(IRoomCommandService commandService, IImageStorageService imageStorage)
        {
            _commandService = commandService;
             _imageStorage = imageStorage;
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

  [HttpPost("{roomId:guid}/images/upload")]
public async Task<IActionResult> UploadImage(
    Guid roomId,
    IFormFile file,
    [FromQuery] bool isCover = false,
    [FromQuery] int displayOrder = 0)
{
    if (file == null || file.Length == 0)
        return BadRequest("No file uploaded");

    if (file.Length > MaxFileSize)
        return BadRequest("File too large. Max 5MB allowed.");

    if (!AllowedTypes.Contains(file.ContentType))
        return BadRequest("Only JPEG, PNG, and WEBP images are allowed.");

    // API converts IFormFile → Stream and passes clean data down
    await using var stream = file.OpenReadStream();

    var imageUrl = await _imageStorage.UploadAsync(
        stream,
        file.FileName,
        file.ContentType,
        $"moore-hotel/rooms/{roomId}"
    );

    var dto = new CreateRoomImageDto
    {
        ImageUrl = imageUrl,
        IsCover = isCover,
        DisplayOrder = displayOrder
    };

    await _commandService.AddImageAsync(roomId, dto);

    return Ok(new
    {
        imageUrl,
        isCover,
        displayOrder
    });
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
         
         [HttpDelete("images/{imageId:guid}")]
            public async Task<IActionResult> DeleteImage(Guid imageId)
            {
                await _commandService.DeleteImageAsync(imageId);
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
