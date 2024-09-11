using Domain.Entities;
using MediatR;
using OasisAPI.Enums;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class ContinueConversationWithChatBotsCommand : IRequest<AppResult<IEnumerable<OasisMessage>>>
{
    public string Message { get; }
    public HashSet<ChatBotsEnum> ChatBotsEnums { get; }
    
    public ContinueConversationWithChatBotsCommand(string message, HashSet<ChatBotsEnum> chatBotsEnums)
    {
        Message = message;
        ChatBotsEnums = chatBotsEnums;
    }
}