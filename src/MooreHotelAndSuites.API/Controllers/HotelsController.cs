using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelsController(IHotelService hotelService) => _hotelService = hotelService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _hotelService.GetAllAsync());
    }
}
