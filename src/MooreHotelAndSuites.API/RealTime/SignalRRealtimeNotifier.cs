using Microsoft.AspNetCore.SignalR;
using MooreHotelAndSuites.API.Hubs;
using MooreHotelAndSuites.Application.Interfaces.Realtime;

namespace MooreHotelAndSuites.API.Realtime
{
    public class SignalRRealtimeNotifier : IRealtimeNotifier
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRRealtimeNotifier(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task BroadcastAsync(string channel, object payload)
        {
            await _hub.Clients.Group(channel)
                .SendAsync("receive", payload);
        }
    }
}
