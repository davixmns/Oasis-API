using Domain.Entities;
using MediatR;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class GetAllUserChatsQuery : IRequest<AppResult<IEnumerable<OasisChat>>>
{
    public int UserId { get; }

    public GetAllUserChatsQuery(int userId)
    {
        UserId = userId;
    }
}