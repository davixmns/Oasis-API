using Domain.Entities;
using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients;

public interface IChatGptClient
{
    Task<ChatBotMessageDto> CreateThreadAndSendMessageAsync(string message);
    Task<ChatBotMessageDto> SendMessageToThreadAsync(string threadId, string userMessage);
}