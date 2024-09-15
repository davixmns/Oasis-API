using Domain.Entities;
using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients;

public interface IGeminiClient
{
    Task<ChatBotMessageDto> CreateThreadAndSendMessageAsync(string message);
    Task<ChatBotMessageDto> SendMessageToThreadAsync(IEnumerable<string> chatMessages);
    Task<ChatBotMessageDto> GetChatTitleAsync(string userMessage);
}