using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.DTOs.Booking;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
    {
        var guestId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var booking = await _bookingService.CreateAsync(dto, guestId);

        return Ok(booking);
    }



        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var booking = await _bookingService.GetAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }
    }
}
