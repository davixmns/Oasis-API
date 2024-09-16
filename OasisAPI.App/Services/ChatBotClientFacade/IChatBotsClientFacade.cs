using Domain.Entities;
using Domain.Utils;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Interfaces.Services;

public interface IChatBotsClientFacade
{
    Task<IEnumerable<ChatBotMessageDto>> StartConversationWithChatBots(string message, HashSet<ChatBotEnum> selectedChatBots);

    Task<IEnumerable<ChatBotMessageDto>> ContinueConversationWithChatBotsAsync(string message, IList<string> allMessages, HashSet<OasisChatBotDetails> chatBotDetailsSet);
}