using Domain.Utils;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Features.ChatBot.Commands.StartConversationWithChatBots;

public class StartConversationWithChatBotsCommand : IRequest<AppResult<IEnumerable<ChatBotMessageDto>>>
{
    public string Message { get; }
    public HashSet<ChatBotEnum> ChatBotsEnums { get; }
    
    public StartConversationWithChatBotsCommand(string message, HashSet<ChatBotEnum> chatBotsEnums)
    {
        Message = message;
        ChatBotsEnums = chatBotsEnums;
    }
}