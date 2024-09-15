using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Commands;

public class ContinueConversationWithChatBotsCommand : IRequest<AppResult<IEnumerable<ChatBotMessageDto>>>
{
    public int OasisChatId { get; }
    public string Message { get; }
    public HashSet<ChatBotEnum> ChatBotsEnums { get; }
    
    public ContinueConversationWithChatBotsCommand(int oasisChatId, string message, HashSet<ChatBotEnum> chatBotsEnums)
    {
        OasisChatId = oasisChatId;
        Message = message;
        ChatBotsEnums = chatBotsEnums;
    }
}