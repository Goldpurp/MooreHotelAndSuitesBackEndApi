using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _bookingService;
        public BookingsController(BookingService bookingService) => _bookingService = bookingService;

        [HttpPost]
        public async Task<IActionResult> Create(Booking b)
        {
            b.Reference = "BK-" + Guid.NewGuid().ToString().Split('-')[0].ToUpper();
            await _bookingService.CreateAsync(b);
            return Created(string.Empty, b);
        }

            [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var res = await _bookingService.GetAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

    }
}
