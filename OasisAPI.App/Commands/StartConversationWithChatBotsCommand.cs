using Domain.Entities;
using MediatR;
using OasisAPI.Enums;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class StartConversationWithChatBotsCommand : IRequest<AppResult<IEnumerable<OasisMessage>>>
{
    public string Message { get; }
    public HashSet<ChatBotsEnum> ChatBotsEnums { get; }
    
    public StartConversationWithChatBotsCommand(string message, HashSet<ChatBotsEnum> chatBotsEnums)
    {
        Message = message;
        ChatBotsEnums = chatBotsEnums;
    }
}