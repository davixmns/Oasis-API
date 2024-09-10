using Domain.Entities;

namespace OasisAPI.Infra.Clients;

public interface IChatGptClient
{
    Task<OasisMessage> CreateChatAndSendMessage(string userMessage);
    Task<OasisMessage> SendMessageToChat(string threadId, string userMessage);
}