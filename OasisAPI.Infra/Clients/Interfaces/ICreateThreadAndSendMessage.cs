using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients.Interfaces;

public interface ICreateThreadAndSendMessage
{
    Task<ChatBotMessageDto> CreateThreadAndSendMessageAsync(string message);
}