using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients.Interfaces;

public interface ISendMessageToThread
{
    Task<ChatBotMessageDto> SendMessageToThreadAsync(string threadId, string message);
}