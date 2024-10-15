using Domain.Utils;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Features.ChatBot.Commands.StartConversationWithChatBots;

public class StartConversationWithChatBotsCommand : IRequest<AppResult<IEnumerable<ChatBotMessageDto>>>
{
    public string Message { get; }
    
    public StartConversationWithChatBotsCommand(string message)
    {
        Message = message;
    }
}