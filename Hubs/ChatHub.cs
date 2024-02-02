using Microsoft.AspNetCore.SignalR;

namespace SignalR_Tutorial.Hubs
{
    public class ChatHub : Hub
    {
        #region Common 
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public override async Task OnConnectedAsync()
        {
            // Perform actions when a client connects.
            // For example, you can notify other clients about the new connection.
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Perform actions when a client disconnects.
            // For example, you can notify other clients about the disconnection.
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendPrivateMessage(string sender, string receiver, string message)
        {
            await Clients.User(receiver).SendAsync("ReceivePrivateMessage", sender, message);
        }
        public async Task SendToUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }
        //You can also send messages to specific connections directly:
        public async Task SendToConnection(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        #endregion
        #region Groups
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", Context.ConnectionId, message);
        }
        #endregion

        #region Callers
        // You can send messages directly to the client that initiated the request.
        // In your ChatHub.cs, you can add methods like SendToCaller:
        public async Task SendToCaller(string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }
        // If you want to broadcast a message to all clients
        // except the one who triggered the action, you can use Others
        public async Task SendToAllExceptCaller(string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        #endregion
    }
}
