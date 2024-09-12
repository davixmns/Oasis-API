using Domain.Entities;
using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients;

public interface IChatGptClient
{
    Task<ChatBotMessageResponseDto> CreateThreadAndSendMessageAsync(string message);
    Task<ChatBotMessageResponseDto> SendMessageToThreadAsync(string threadId, string userMessage);
}