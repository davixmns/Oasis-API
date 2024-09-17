using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Features.Auth.Login;

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