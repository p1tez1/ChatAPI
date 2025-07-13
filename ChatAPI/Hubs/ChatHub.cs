using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

[Authorize]
public class ChatHub : Hub
{
    private static ConcurrentDictionary<string, string> ConnectedUsers = new();
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        string user = Context.GetHttpContext().Request.Query["user"];
        ConnectedUsers.TryAdd(Context.ConnectionId, user);

        await Clients.All.SendAsync("UserCountUpdated", ConnectedUsers.Count);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        ConnectedUsers.TryRemove(Context.ConnectionId, out _);

        await Clients.All.SendAsync("UserCountUpdated", ConnectedUsers.Count);
        await base.OnDisconnectedAsync(exception);
    }
}
