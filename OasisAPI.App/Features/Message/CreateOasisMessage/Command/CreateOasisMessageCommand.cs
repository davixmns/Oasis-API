using Domain.Entities;
using Domain.Utils;
using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Commands;

public class CreateOasisMessageCommand : IRequest<AppResult<OasisMessage>>
{
    public int OasisChatId { get; }
    public string Message { get; }
    public ChatBotEnum From { get; }
    
    public CreateOasisMessageCommand(int oasisChatId, string message, ChatBotEnum from)
    {
        OasisChatId = oasisChatId;
        Message = message;
        From = from;
    }
}