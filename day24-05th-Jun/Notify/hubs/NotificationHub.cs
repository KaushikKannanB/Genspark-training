using System.Configuration;
using Microsoft.AspNetCore.SignalR;

namespace Notify.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("Received Message", user, message);
        }
    }
    
}