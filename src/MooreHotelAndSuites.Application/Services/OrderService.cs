using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Application.Interfaces.Events;
using MooreHotelAndSuites.Application.DTOs.Order;
using MooreHotelAndSuites.Application.DTOs.Laundry;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Domain.Events;

namespace MooreHotelAndSuites.Application.Services
{
public class OrderService : IOrderService
{  
      private readonly IOrderRepository _orderRepo;
    private readonly IBookingRepository _bookingRepo;
    private readonly IMenuRepository _menuRepo;
    private readonly IGuestRepository _guestRepo;
    private readonly IDomainEventDispatcher _dispatcher;

public OrderService(
    IOrderRepository orderRepo,
    IBookingRepository bookingRepo,
    IMenuRepository menuRepo,
    IGuestRepository guestRepo,
    IDomainEventDispatcher dispatcher)
{
    _orderRepo = orderRepo;
    _bookingRepo = bookingRepo;
    _menuRepo = menuRepo;
    _guestRepo = guestRepo;
    _dispatcher = dispatcher;
}

public async Task<(Guid id, decimal amount)>
    CreateOrderAsync(CreateOrderDto dto)
{
    ServiceOrder order;

    if (dto.Source == OrderSource.EventHall)
    {
        order = ServiceOrder.CreateForEventWalkIn(
            dto.CustomerName!,
            dto.PhoneNumber!);
    }
    else
    {
        var booking = await _bookingRepo
            .GetActiveByGuestAsync(dto.CustomerName!, dto.PhoneNumber!);

        if (booking == null)
            throw new Exception("No active guest found");

        order = ServiceOrder.CreateForHotelGuest(
            booking,
            dto.CustomerName!,
            dto.PhoneNumber!,
            dto.Source);
    }

    foreach (var item in dto.Items)
    {
        var menu = await _menuRepo.GetByIdAsync(item.MenuItemId);

        if (menu == null)
            throw new Exception($"Menu item {item.MenuItemId} not found");

        order.AddItem(menu, item.Quantity);
    }

  
    await _orderRepo.AddAsync(order);

   
    await _dispatcher.DispatchAsync(new[]
    {
        new OrderCreatedEvent(order.Id, order.Source)
    });

    return (order.Id, order.TotalAmount);
}


public async Task<(Guid id, decimal amount)>
    CreateLaundryOrderAsync(CreateLaundryOrderDto dto)
{
    var booking = await _bookingRepo
        .GetActiveByGuestAsync(dto.CustomerName, dto.PhoneNumber);

    if (booking == null)
        throw new Exception("Only checked-in guests can use laundry");

    var order = ServiceOrder.CreateLaundry(
        booking,
        dto.CustomerName,
        dto.PhoneNumber);

    foreach (var item in dto.Items)
    {
        order.AddLaundryItem(
            item.Type,
            item.Quantity,
            item.Description);
    }

    await _orderRepo.AddAsync(order);


    await _dispatcher.DispatchAsync(new[]
    {
        new OrderCreatedEvent(order.Id, OrderSource.Laundry)
    });

    return (order.Id, order.TotalAmount);
}


public async Task ConfirmPaymentAsync(ConfirmOrderPaymentDto dto)
{
    var order = await _orderRepo
        .GetPendingByCustomerAsync(
            dto.CustomerName,
            dto.PhoneNumber,
            dto.Amount);

    if (order == null)
        throw new Exception("Pending order not found");

    order.ConfirmPayment(dto.GuestId);

    await _dispatcher.DispatchAsync(order.DomainEvents);
}


// ===== MARK SERVED =====
public async Task MarkServedAsync(ServeOrderDto dto)
{
    var order = await _orderRepo
        .GetActiveForServingAsync(
            dto.CustomerName,
            dto.PhoneNumber);

    if (order == null)
        throw new Exception("No confirmed order found");

    order.MarkServed();
}

}
}
