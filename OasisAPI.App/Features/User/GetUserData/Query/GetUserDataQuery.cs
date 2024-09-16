using MediatR;
using OasisAPI.App.Dto.Response;
using OasisAPI.App.Result;

namespace OasisAPI.App.Commands;

public class GetUserDataQuery : IRequest<AppResult<OasisUserResponseDto>>
{
    public int UserId { get; }

    public GetUserDataQuery(int userId)
    {
        UserId = userId;
    }
}