using Microsoft.AspNetCore.SignalR;

namespace Inventory.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("Received Message", user, message);
        }
    }
}