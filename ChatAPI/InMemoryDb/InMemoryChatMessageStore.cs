using ChatAPI.Models;
public class InMemoryChatMessageStore : IChatMessageStore
{
    private readonly List<ChatMessage> _messages = new();

    public void Add(ChatMessage message)
    {
        _messages.Add(message);
    }

    public IReadOnlyList<ChatMessage> GetAll() => _messages;
}
