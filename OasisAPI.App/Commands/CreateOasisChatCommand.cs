using Domain.Entities;
using MediatR;
using OasisAPI.App.Dto.Request;
using OasisAPI.App.Result;

namespace OasisAPI.App.Commands;

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