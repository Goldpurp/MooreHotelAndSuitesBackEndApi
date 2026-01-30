using MooreHotelAndSuites.Application.DTOs.Analytics;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;

namespace MooreHotelAndSuites.Application.Services
{
    public class AuditAnalyticsService : IAuditAnalyticsService
    {
        private readonly IAuditAnalyticsRepository _repo;

        public AuditAnalyticsService(IAuditAnalyticsRepository repo)
        {
            _repo = repo;
        }

        public Task<int> CountActionsAsync(string action)
            => _repo.CountByActionAsync(action);

        public Task<List<ActionCountDto>> ActionsPerDayAsync(DateTime from)
            => _repo.ActionsPerDayAsync(from);
    }
}
