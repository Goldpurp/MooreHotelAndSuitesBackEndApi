using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Domain.Entities;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly HotelService _hotelService;
        public HotelsController(HotelService hotelService) => _hotelService = hotelService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _hotelService.GetAllAsync());
    }
}
