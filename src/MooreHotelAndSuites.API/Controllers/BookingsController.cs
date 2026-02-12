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
using MooreHotelAndSuites.Domain.Constants;

namespace MooreHotelAndSuites.API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
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

       [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            [FromBody] CreateBookingRequestDto dto)
        {
            var bookingId = await _bookingService.CreateBookingAsync(dto);

            return Ok(new
            {
                id = bookingId,
                message = "Booking created and awaiting payment confirmation"
            });
        }

        [Authorize(Roles = Roles.Receptionist + "," + Roles.Manager)]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _repo.GetAllPendingAsync();

            var result = list.Select(b => new
            {
                b.Id,
                b.Reference,
                b.CheckIn,
                b.CheckOut,
                GuestId = b.GuestId,
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt
            });

            return Ok(result);
        }




[Authorize(Roles = Roles.Receptionist + "," + Roles.Manager)]
[HttpPost("confirm-payment")]
public async Task<IActionResult> ConfirmPayment(
    [FromBody] ConfirmPaymentDto dto)
{
    var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (staffId is null)
        return Unauthorized();

    // Find pending booking using name or phone from credit alert
    var booking = await _bookingService.FindPendingForConfirmationAsync(
        dto.GuestFullName,
        dto.GuestPhoneNumber
    );

    if (booking == null)
        return BadRequest("No matching pending booking");

    var guest = await _guestService.GetByIdAsync(booking.GuestId);
    if (guest == null)
        return BadRequest("Guest not found");

    // DOMAIN ACTION
    booking.AddPayment(
        amount: dto.Amount,
        paymentMethod: dto.PaymentMethod,
        staffId: staffId,
        guestFullName: guest.FullName
    );

    booking.Reserve();

    await _repo.UpdateAsync(booking);
    await _repo.SaveChangesAsync();   

    return Ok(new
    {
        booking.Id,
        booking.Reference,
        Guest = guest.FullName,
        Status = booking.Status.ToString(),
        AmountPaid = booking.AmountPaid,
        TotalAmount = booking.TotalAmount,
        message = "Payment confirmed – room reserved"
    });
}





        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var booking = await _bookingService.GetAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }
       
       [Authorize(Roles = Roles.Receptionist)]
        [HttpPost("{id:guid}/checkin")]
        public async Task<IActionResult> CheckIn(Guid id)
        {
            await _bookingService.CheckInAsync(id);
            return NoContent();
        }

        [Authorize(Roles = Roles.Receptionist)]
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
