using OasisAPI.Infra.Dto;

namespace OasisAPI.Infra.Clients.Interfaces;

public interface IGetChatTitle
{
    Task<ChatBotMessageDto> GetChatTitleAsync(string message); 
}