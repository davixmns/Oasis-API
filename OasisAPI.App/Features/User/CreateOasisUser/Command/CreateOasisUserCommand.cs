using MediatR;
using OasisAPI.App.Dto.Response;
using OasisAPI.App.Result;

namespace OasisAPI.App.Commands;

public class CreateOasisUserCommand : IRequest<AppResult<OasisUserResponseDto>>
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    
    public CreateOasisUserCommand(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}