namespace OasisAPI.App.Config;

public class JwtConfig
{
    public string SecretKey { get; set; }
    public string  Issuer { get; set; }
    public string Audience { get; set; }
    public double AccessTokenExpiry { get; set; }
    public double RefreshTokenExpiry { get; set; }
    
    public JwtConfig(string secretKey, string issuer, string audience, double accessTokenExpiry, double refreshTokenExpiry)
    {
        SecretKey = secretKey;
        Issuer = issuer;
        Audience = audience;
        AccessTokenExpiry = accessTokenExpiry;
        RefreshTokenExpiry = refreshTokenExpiry;
    }
}