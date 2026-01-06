using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : ControllerBase
    {
        private readonly GuestService _guestService;
        public GuestsController(GuestService guestService) => _guestService = guestService;

        [HttpPost]
        public async Task<IActionResult> Create(Guest g) { await _guestService.CreateAsync(g); return Created(string.Empty, g); }
    }
}
