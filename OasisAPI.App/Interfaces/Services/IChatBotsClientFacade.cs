using Domain.Entities;
using OasisAPI.Enums;

namespace OasisAPI.App.Interfaces.Services;

public interface IChatBotsClientFacade
{
    Task<IEnumerable<OasisMessage>> CreateThreadsAndSendMessageAsync(string message, HashSet<ChatBotsEnum> selectedChatBots);
    Task<IEnumerable<OasisMessage>> SendMessageToThreadsAsync(string message, HashSet<ChatBotsEnum> chatBotsEnums);
}