namespace OasisAPI.App.Dto.Response;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryDateTime { get; set; }
    public OasisUserResponseDto OasisUserResponse { get; set; } = null!;
}