using Domain.Entities;
using MediatR;
using OasisAPI.Infra.Dto;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class StartConversationWithChatBotsCommand : IRequest<AppResult<IEnumerable<ChatBotMessageResponseDto>>>
{
    public string Message { get; }
    public HashSet<ChatBotEnum> ChatBotsEnums { get; }
    
    public StartConversationWithChatBotsCommand(string message, HashSet<ChatBotEnum> chatBotsEnums)
    {
        Message = message;
        ChatBotsEnums = chatBotsEnums;
    }
}