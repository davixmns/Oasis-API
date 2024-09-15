using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Commands;

public class GetAllUserChatsQuery : IRequest<AppResult<IEnumerable<OasisChat>>>
{
    public int UserId { get; }

    public GetAllUserChatsQuery(int userId)
    {
        UserId = userId;
    }
}