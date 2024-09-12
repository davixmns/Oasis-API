using Domain.Entities;
using MediatR;
using OasisAPI.App.Dto.Request;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class CreateChatCommand : IRequest<AppResult<OasisChat>>
{
    public int OasisUserId { get; }
    public string Title { get; }
    public string InitialMessage { get; }
    
    public CreateChatCommand(int oasisUserId, string title, string initialMessage)
    {
        OasisUserId = oasisUserId;
        Title = title;
        InitialMessage = initialMessage;
    }
}