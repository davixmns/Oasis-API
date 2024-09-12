using Domain.Entities;
using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients;

public interface IGeminiClient
{
    Task<ChatBotMessageResponseDto> CreateThreadAndSendMessageAsync(string message);
    Task<ChatBotMessageResponseDto> SendMessageToThreadAsync(IEnumerable<string> chatMessages);
    Task<ChatBotMessageResponseDto> GetChatTitleAsync(string userMessage);
}