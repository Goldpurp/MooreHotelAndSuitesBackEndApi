using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Infrastructure.Data;

namespace MooreHotelAndSuites.Infrastructure.Persistence.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly AppDbContext _context;

        public MenuRepository(AppDbContext context)
        {
            _context = context;
        }


    public async Task<List<MenuItem>> GetBySourceAsync(OrderSource source)
    {
        return await _context.MenuItems
            .Where(x => x.Source == source)
            .ToListAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(Guid id)
    {
        return await _context.MenuItems.FindAsync(id);
    }

    public async Task AddAsync(MenuItem item)
    {
        await _context.MenuItems.AddAsync(item);
    }
}

    }

