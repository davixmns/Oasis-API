using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients.Interfaces;

public interface ISendAllMessagesToThread
{
    Task<ChatBotMessageDto> SendAllMessagesAsync(IEnumerable<string> allMessages);
}