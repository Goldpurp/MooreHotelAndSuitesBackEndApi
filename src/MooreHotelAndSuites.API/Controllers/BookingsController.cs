using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.DTOs.Booking;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var booking = await _bookingService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = booking.Id }, booking);
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
