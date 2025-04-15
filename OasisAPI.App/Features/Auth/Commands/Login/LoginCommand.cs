using MediatR;
using OasisAPI.App.Result;

namespace OasisAPI.App.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AppResult<LoginResponseDto>>
{
    public string Email { get; }
    public string Password { get; }
    
    public LoginCommand(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Email and password are required");
        
        Email = email;
        Password = password;
    }
}