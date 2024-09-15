using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Commands;

public class CreateChatTitleCommand : IRequest<AppResult<IEnumerable<object>>>
{
    public int UserId { get; }
}