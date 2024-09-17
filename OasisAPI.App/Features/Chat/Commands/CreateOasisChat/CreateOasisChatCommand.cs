using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Features.Chat.Commands.CreateOasisChat;

public class CreateOasisChatCommand : IRequest<AppResult<OasisChat>>
{
    public int OasisUserId { get; }
    public string Title { get; }
    public string InitialMessage { get; }
    
    public CreateOasisChatCommand(int oasisUserId, string title, string initialMessage)
    {
        OasisUserId = oasisUserId;
        Title = title;
        InitialMessage = initialMessage;
    }
}