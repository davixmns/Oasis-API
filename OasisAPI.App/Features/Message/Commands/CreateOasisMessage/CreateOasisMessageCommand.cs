using Domain.Entities;
using Domain.Utils;
using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Features.Message.Commands.CreateOasisMessage;

public class CreateOasisMessageCommand : IRequest<AppResult<OasisMessage>>
{
    public int OasisChatId { get; }
    public string Message { get; }
    public ChatBotEnum ChatBotEnum { get; }
    
    public CreateOasisMessageCommand(int oasisChatId, string message, ChatBotEnum chatBotEnum)
    {
        OasisChatId = oasisChatId;
        Message = message;
        ChatBotEnum = chatBotEnum;
    }
}