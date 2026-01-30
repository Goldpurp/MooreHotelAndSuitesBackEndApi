namespace MooreHotelAndSuites.Application.Interfaces.Repositories
{
public interface IBookingReadRepository
{
    Task<int> CountActiveBookingsAsync();
    Task<int> CountTodayAsync();
    Task<decimal> SumRevenueTodayAsync();
}


}
