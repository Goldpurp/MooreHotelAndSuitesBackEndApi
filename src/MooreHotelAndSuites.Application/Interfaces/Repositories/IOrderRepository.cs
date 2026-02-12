using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Domain.Enums;

namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(ServiceOrder order);

        Task<List<ServiceOrder>> GetBySourceAsync(
            OrderSource source);

        Task<List<ServiceOrder>> GetConfirmedBySourcesAsync(
            params OrderSource[] sources);

        Task<ServiceOrder?> GetPendingByCustomerAsync(
            string name,
            string phone,
            decimal amount);

        Task<ServiceOrder?> GetActiveForServingAsync(
            string name,
            string phone);

        Task<List<ServiceOrder>> GetPendingBySourceAsync(
            OrderSource source);

        Task<ServiceOrder?> GetByIdAsync(Guid id);
    }
}
