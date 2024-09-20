using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Features.Chat.Commands.UpdateOasisChatDetails;

public class UpdateOasisChatBotDetailsCommand : IRequest<AppResult<Unit>>
{
    public int OasisChatBotDetailsId { get; init; }
    public bool IsActive { get; init; }
    
    public UpdateOasisChatBotDetailsCommand(int oasisChatBotDetailsId, bool isActive)
    {
        OasisChatBotDetailsId = oasisChatBotDetailsId;
        IsActive = isActive;
    }
}