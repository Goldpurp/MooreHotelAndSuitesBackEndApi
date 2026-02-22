using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
 public interface IMenuRepository
{
    Task<MenuItem?> GetByIdAsync(Guid id);

    Task<List<MenuItem>> GetBySourceAsync(OrderSource source);

    Task AddAsync(MenuItem item);
}


}