namespace MooreHotelAndSuites.Application.Interfaces.Realtime
{
    public interface IRealtimeNotifier
    {
        Task BroadcastAsync(string channel, object payload);
    }
}
