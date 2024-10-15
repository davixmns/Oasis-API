using Domain.Utils;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Features.ChatBot.Commands.ContinueConversationWithChatBots;

public class ContinueConversationWithChatBotsCommand : IRequest<AppResult<IEnumerable<ChatBotMessageDto>>>
{
    public int OasisChatId { get; }
    public string Message { get; }
    
    public ContinueConversationWithChatBotsCommand(int oasisChatId, string message)
    {
        OasisChatId = oasisChatId;
        Message = message;
    }
}