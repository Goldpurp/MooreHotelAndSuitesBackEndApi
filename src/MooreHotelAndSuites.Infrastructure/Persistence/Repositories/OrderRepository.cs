using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Infrastructure.Data;

namespace MooreHotelAndSuites.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ServiceOrder order)
        {
            await _context.Set<ServiceOrder>().AddAsync(order);
        }

        public async Task<List<ServiceOrder>> GetBySourceAsync(
            OrderSource source)
        {
            return await _context.Set<ServiceOrder>()
                .Include(x => x.Items)
                .Where(x => x.Source == source)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
      
        public async Task<List<ServiceOrder>> GetConfirmedBySourcesAsync(
            params OrderSource[] sources)
        {
            return await _context.Set<ServiceOrder>()
                .Include(x => x.Items)
                .Where(x =>
                    sources.Contains(x.Source) &&
                    x.Status == OrderStatus.Confirmed)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        
public async Task<ServiceOrder?> GetPendingByCustomerAsync(
    string name,
    string phone,
    decimal amount)
{
    return await _context.ServiceOrders
        .Include(x => x.Items)
        .Where(x =>
            x.CustomerName == name &&
            x.PhoneNumber == phone &&
            x.Status == OrderStatus.PendingPayment &&
            x.TotalAmount == amount)
        .FirstOrDefaultAsync();
}
public async Task<ServiceOrder?> GetActiveForServingAsync(
    string name,
    string phone)
{
    return await _context.ServiceOrders
        .Include(x => x.Items)
        .Where(x =>
            x.CustomerName == name &&
            x.PhoneNumber == phone &&
            x.Status == OrderStatus.Confirmed)
        .OrderBy(x => x.CreatedAt)   // serve oldest first
        .FirstOrDefaultAsync();
}

        public async Task<List<ServiceOrder>> GetPendingBySourceAsync(
            OrderSource source)
        {
            return await _context.Set<ServiceOrder>()
                .Include(x => x.Items)
                .Where(x =>
                    x.Source == source &&
                    x.Status == OrderStatus.PendingPayment)
                .ToListAsync();
        }

        public async Task<ServiceOrder?> GetByIdAsync(Guid id)
        {
            return await _context.Set<ServiceOrder>()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
