namespace OasisAPI.App.Dto.Request;

public class TokenRequestDto
{ 
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}