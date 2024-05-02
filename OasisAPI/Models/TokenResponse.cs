using OasisAPI.Dto;

namespace OasisAPI.Models;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDateTime { get; set; }
    public OasisUserDto OasisUser { get; set; }
}