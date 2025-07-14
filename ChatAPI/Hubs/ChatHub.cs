using ChatAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

[Authorize]
public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new();

    private static readonly List<ChatMessage> MessageHistory = new();
    private static readonly object _lock = new();

    public async Task SendMessage(string user, string message)
    {
        var msg = new ChatMessage
        {
            User = user,
            Text = message,
            Timestamp = DateTime.UtcNow
        };

        lock (_lock)
        {
            MessageHistory.Add(msg);
        }

        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        string user = Context.GetHttpContext().Request.Query["user"];
        ConnectedUsers.TryAdd(Context.ConnectionId, user);

        List<ChatMessage> history;
        lock (_lock)
        {
            history = MessageHistory.ToList();
        }

        foreach (var msg in history)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", msg.User, msg.Text);
        }

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
