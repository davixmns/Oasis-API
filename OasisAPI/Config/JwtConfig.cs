using System.ComponentModel.DataAnnotations;

namespace OasisAPI.Config;

public class JwtConfig
{
    public string SecretKey { get; set; } = string.Empty;
    public string  Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public double AccessTokenExpiry { get; set; }
    public double RefreshTokenExpiry { get; set; }
}