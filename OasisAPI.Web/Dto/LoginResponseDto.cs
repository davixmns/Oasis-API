namespace OasisAPI.Dto;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDateTime { get; set; }
    public OasisUserResponseDto OasisUserResponse { get; set; }
}