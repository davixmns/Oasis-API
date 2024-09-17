using OasisAPI.App.Features.User.Queries.GetUserData;

namespace OasisAPI.App.Features.Auth.Commands.Login;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDateTime { get; set; }
    public OasisUserResponseDto OasisUserResponse { get; set; }
}