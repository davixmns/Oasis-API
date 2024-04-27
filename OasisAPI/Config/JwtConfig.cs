using System.ComponentModel.DataAnnotations;

namespace OasisAPI.Config;

public class JwtConfig
{
    public string? SecretKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public double? AccessTokenExpiry { get; set; }
    public double? RefreshTokenExpiry { get; set; }
}