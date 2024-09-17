using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Features.User.Queries.GetUserData;

public class GetUserDataQuery : IRequest<AppResult<OasisUserResponseDto>>
{
    public int UserId { get; }

    public GetUserDataQuery(int userId)
    {
        UserId = userId;
    }
}