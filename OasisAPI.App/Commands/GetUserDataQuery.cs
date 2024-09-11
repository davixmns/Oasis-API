using MediatR;
using OasisAPI.App.Dto.Response;
using OasisAPI.Models;

namespace OasisAPI.App.Commands;

public class GetUserDataQuery : IRequest<AppResult<OasisUserResponseDto>>
{
    public int UserId { get; }

    public GetUserDataQuery(int userId)
    {
        UserId = userId;
    }
}