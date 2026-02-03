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
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequestDto dto)
    {
        // Remove guestId from here
        var bookingId = await _bookingService.CreateBookingAsync(dto);

        return Ok(new { id = bookingId });
    }





        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var booking = await _bookingService.GetAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }
        [HttpPost("draft")]
        public async Task<IActionResult> CreateDraft([FromBody] CreateBookingRequestDto dto)
        {
            var id = await _bookingService.CreateDraftAsync(dto);
            return Ok(new { id });
        }

        [HttpPost("{id:guid}/checkin")]
        public async Task<IActionResult> CheckIn(Guid id)
        {
            await _bookingService.CheckInAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _bookingService.CancelAsync(id);
            return NoContent();
        }

    }
}
