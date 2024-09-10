namespace OasisAPI.App.Dto.Request;

public class CreateOasisUserRequestDto
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}