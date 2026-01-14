using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Application.Services;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomQueryService _query;

        public RoomsController(IRoomQueryService query)
        {
            _query = query;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _query.GetAllAsync());
            

            [HttpGet("{id:guid}", Name = "GetRoomById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _query.GetByIdAsync(id);
            return room == null ? NotFound() : Ok(room);
        }


        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable(
            DateTime checkIn,
            DateTime checkOut,
            int guests)
        {
            return Ok(await _query
                .GetAvailableAsync(checkIn, checkOut, guests));
        }
    }
}
