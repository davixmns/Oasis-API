using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Features.Chat.Commands.InitializeOasisChatDetails;

public class InitializeOasisChatBotDetailsCommand : IRequest<AppResult<OasisChat>>
{
    public OasisChat OasisChat { get; }
    public IEnumerable<ChatBotMessageDto> Messages { get; }
    
    public InitializeOasisChatBotDetailsCommand(OasisChat oasisChat, IEnumerable<ChatBotMessageDto> messages)
    {
        OasisChat = oasisChat;
        Messages = messages;
    }
}