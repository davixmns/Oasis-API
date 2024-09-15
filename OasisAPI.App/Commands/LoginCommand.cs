using MediatR;
using OasisAPI.App.Dto.Response;
using OasisAPI.App.Result;

namespace OasisAPI.App.Commands;

public class LoginCommand : IRequest<AppResult<LoginResponseDto>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    public LoginCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}