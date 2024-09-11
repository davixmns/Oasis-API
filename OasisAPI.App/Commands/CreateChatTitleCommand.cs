using Domain.Entities;
using MediatR;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class CreateChatTitleCommand : IRequest<AppResult<IEnumerable<object>>>
{
    public int UserId { get; }
}