using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SimpleMessagesSignalR
{
    public class MessageHub : Hub
    {
        public async Task Register(string key)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, key);
        }

        public async Task SendMessage(string key, object message)
        {
            await Clients.Group(key).SendAsync("ReceiveMessage", message);
        }
    }
}
