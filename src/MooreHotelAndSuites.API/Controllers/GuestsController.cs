using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.DTOs.Guests;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : ControllerBase
    {
        private readonly IGuestService _guestService;

        public GuestsController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var guest = await _guestService.GetByIdAsync(id);
            if (guest == null)
                return NotFound();

            return Ok(guest);
        }
    }
}
