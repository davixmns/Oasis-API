namespace OasisAPI.App.Features.User.Commands.CreateOasisUser;

public class CreateOasisUserRequestDto
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}