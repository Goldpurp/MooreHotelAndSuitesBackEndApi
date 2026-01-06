using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomsController(RoomService roomService)
        {
            _roomService = roomService;
        }

        // =====================
        // GUEST ENDPOINTS
        // =====================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut, [FromQuery] int guests)
        {
            var rooms = await _roomService.GetAvailableRoomsAsync(checkIn, checkOut, guests);
            return Ok(rooms);
        }

        // =====================
        // ADMIN ENDPOINTS
        // =====================
        // [Authorize(Roles = "Admin,Manager,Receptionist")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Room room)
        {
            await _roomService.CreateAsync(room);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        // [Authorize(Roles = "Admin,Manager,Receptionist")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] RoomStatus status)
        {
            await _roomService.UpdateStatusAsync(id, status);
            return NoContent();
        }

        // [Authorize(Roles = "Admin,Manager,Receptionist")]
        [HttpPut("{id}/amenities")]
        public async Task<IActionResult> UpdateAmenities(int id, [FromBody] IReadOnlyCollection<int> amenityIds)
        {
            await _roomService.UpdateAmenitiesAsync(id, amenityIds);
            return NoContent();
        }

        // [Authorize(Roles = "Admin,Manager,Receptionist")]
        [HttpPut("{id}/images")]
        public async Task<IActionResult> UpdateImages(int id, [FromBody] IReadOnlyCollection<RoomImage> images)
        {
            await _roomService.UpdateImagesAsync(id, images);
            return NoContent();
        }

        // [Authorize(Roles = "Admin,Manager,Receptionist")]
        [HttpPut("{id}/rating")]
        public async Task<IActionResult> UpdateRating(int id)
        {
            await _roomService.UpdateRatingAsync(id);
            return NoContent();
        }
    }
}
