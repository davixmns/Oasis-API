using Domain.Entities;

namespace OasisAPI.Interfaces.Clients;

public interface IChatGptClient
{
    Task<OasisMessage> CreateChatAndSendMessage(string userMessage);
    Task<OasisMessage> SendMessageToChat(string threadId, string userMessage);
}