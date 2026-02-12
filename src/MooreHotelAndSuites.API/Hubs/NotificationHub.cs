using Microsoft.AspNetCore.SignalR;

namespace MooreHotelAndSuites.API.Hubs
{
    public class NotificationHub : Hub
    {
        // Devices join channels like:
        // "kitchen", "bar", "laundry", "roomservice", "staff"

        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public async Task LeaveGroup(string group)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
