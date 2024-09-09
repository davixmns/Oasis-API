using Domain.Entities;

namespace OasisAPI.Interfaces.Clients;

public interface IGeminiClient
{
    Task<OasisMessage> CreateChatAndSendMessageAsync(string userMessage);
    Task<OasisMessage> SendMessageToChatAsync(IEnumerable<OasisMessage> chatMessages);
    Task<OasisMessage> GetChatTitleAsync(string userMessage);
}