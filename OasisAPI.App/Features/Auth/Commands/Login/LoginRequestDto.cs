namespace OasisAPI.App.Features.Auth.Commands.Login;

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}