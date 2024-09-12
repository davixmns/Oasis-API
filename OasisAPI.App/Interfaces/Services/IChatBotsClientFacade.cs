using Domain.Entities;
using OasisAPI.App.Dto.Request;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Interfaces.Services;

public interface IChatBotsClientFacade
{
    Task<IEnumerable<ChatBotMessageResponseDto>> CreateThreadsAndSendMessageAsync(string message, HashSet<ChatBotEnum> selectedChatBots);

    Task<IEnumerable<ChatBotMessageResponseDto>> SendMessageToThreadsAsync(string message, IList<string> allMessages,
        HashSet<ChatBotAndThreadDto> chatBotAndThreadDtos);
}