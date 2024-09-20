using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQuery : IRequest<AppResult<IEnumerable<OasisMessage>>>
{
    public int OasisChatId { get; init; }
    
    public GetChatMessagesQuery(int oasisChatId)
    {
        OasisChatId = oasisChatId;
    }
}