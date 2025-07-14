using ChatAPI.Models;

public interface IChatMessageStore
{
    void Add(ChatMessage message);
    IReadOnlyList<ChatMessage> GetAll();
}