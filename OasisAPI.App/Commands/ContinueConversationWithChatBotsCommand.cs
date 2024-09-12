using Domain.Entities;
using MediatR;
using OasisAPI.Infra.Dto;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class ContinueConversationWithChatBotsCommand : IRequest<AppResult<IEnumerable<ChatBotMessageResponseDto>>>
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