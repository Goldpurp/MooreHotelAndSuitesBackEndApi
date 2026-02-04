using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.DTOs.Booking;
using MooreHotelAndSuites.Application.DTOs.Payments;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using System.Security.Claims;
using MooreHotelAndSuites.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IGuestService _guestService;
         private readonly IBookingRepository _repo;
        public BookingsController(IBookingService bookingService,
         IBookingRepository repo, IGuestService guestService)
        {
            _bookingService = bookingService;
            _repo  = repo;
            _guestService = guestService;
        }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequestDto dto)
    {
       
        var bookingId = await _bookingService.CreateBookingAsync(dto);

        return Ok(new { id = bookingId });
    }

[HttpPost("confirm-last")]
public async Task<IActionResult> ConfirmLastPending(ConfirmPaymentDto dto)
{
    var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (staffId is null)
        return Unauthorized();

    var booking = await _bookingService.FindPendingForConfirmationAsync(
        dto.GuestFullName,
        dto.GuestPhoneNumber);

    if (booking == null)
        return BadRequest("No matching pending booking");

    var guest = await _guestService.GetByIdAsync(booking.GuestId);

    booking.AddPayment(
        dto.Amount,
        dto.PaymentMethod,
        staffId,
        guest!.FullName
    );

    booking.Reserve();

    await _repo.UpdateAsync(booking);

    return Ok(new
    {
        booking.Id,
        booking.Reference,
        Status = booking.Status.ToString()
    });
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

       [HttpPost("{id:guid}/checkout")]
        public async Task<IActionResult> CheckOut(Guid id)
        {
            await _bookingService.CheckOutAsync(id);
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
