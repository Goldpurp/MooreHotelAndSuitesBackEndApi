using Microsoft.AspNetCore.Mvc;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Application.DTOs.Order;
using MooreHotelAndSuites.Application.DTOs.Laundry;
using MooreHotelAndSuites.Infrastructure.Data;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Application.Interfaces.Identity;


namespace MooreHotelAndSuites.API.Controllers {
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly IOrderRepository _repo;

public OrderController(
    IOrderService service,
    IOrderRepository repo)
{
    _service = service;
    _repo = repo;
}

[HttpPost]
public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
{
    var (id, amount) = await _service.CreateOrderAsync(dto);

    return Ok(new {
        orderId = id,
        amount,
        paymentRequired = true
    });
}

[HttpPost("laundry")]
public async Task<IActionResult> Laundry(CreateLaundryOrderDto dto)
{
    var (id, amount) =
        await _service.CreateLaundryOrderAsync(dto);

    return Ok(new { orderId = id, amount });
}

[HttpPost("confirm-payment")]
public async Task<IActionResult> Confirm(ConfirmOrderPaymentDto dto)
{
    await _service.ConfirmPaymentAsync(dto);
    return Ok("confirmed");
}
[HttpGet("dashboard/kitchen")]
public async Task<IActionResult> Kitchen()
{
    var orders = await _repo.GetConfirmedBySourcesAsync(
        OrderSource.Kitchen,
        OrderSource.RoomService);

    return Ok(orders);
}

[HttpGet("dashboard/bar")]
public async Task<IActionResult> Bar()
{
    var orders = await _repo.GetConfirmedBySourcesAsync(
        OrderSource.Bar);

    return Ok(orders);
}

[HttpGet("dashboard/laundry")]
public async Task<IActionResult> LaundryBoard()
{
    var orders = await _repo.GetConfirmedBySourcesAsync(
        OrderSource.Laundry);

    return Ok(orders);
}

[HttpGet("dashboard/event")]
public async Task<IActionResult> Event()
{
    var orders = await _repo.GetConfirmedBySourcesAsync(
        OrderSource.EventHall);

    return Ok(orders);
}


[HttpPost("serve")]
public async Task<IActionResult> Serve(ServeOrderDto dto)
{
    await _service.MarkServedAsync(dto);
    return Ok("served");
}
}

}
