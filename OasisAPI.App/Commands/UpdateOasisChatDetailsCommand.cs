using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Commands;

public class UpdateOasisChatDetailsCommand : IRequest<AppResult<OasisChat>>
{
    public OasisChat OasisChat { get; }
    public IEnumerable<ChatBotMessageDto> Messages { get; }
    
    public UpdateOasisChatDetailsCommand(OasisChat oasisChat, IEnumerable<ChatBotMessageDto> messages)
    {
        OasisChat = oasisChat;
        Messages = messages;
    }
}