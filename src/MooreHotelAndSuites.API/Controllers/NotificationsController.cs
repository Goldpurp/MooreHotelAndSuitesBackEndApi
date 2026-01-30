using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Interfaces.Services;
using System.Security.Claims;
using MooreHotelAndSuites.Application.Services;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("my")]
        public async Task<IActionResult> My()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await _service.GetMyAsync(userId));
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("staff")]
        public async Task<IActionResult> Staff()
        {
            return Ok(await _service.GetStaffAsync());
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            await _service.MarkAsReadAsync(id);
            return NoContent();
        }
    }
}
