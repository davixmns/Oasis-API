using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OasisAPI.Config;
using OasisAPI.Interfaces;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;

namespace OasisAPI.Services;

public class TokenService : ITokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly double _accessTokenExpiry;

    public TokenService(IOptions<JwtConfig> jwtConfig)
    {
        _secretKey = jwtConfig.Value.SecretKey ?? throw new InvalidOperationException();
        _issuer = jwtConfig.Value.Issuer ?? throw new InvalidOperationException();
        _audience = jwtConfig.Value.Audience ?? throw new InvalidOperationException();
        _accessTokenExpiry = jwtConfig.Value.AccessTokenExpiry ?? throw new InvalidOperationException();
    }

    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var privateKey = Encoding.UTF8.GetBytes(_secretKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpiry),
            Audience = _audience,
            Issuer = _issuer,
            SigningCredentials = credentials
        };
        return new JwtSecurityTokenHandler().CreateJwtSecurityToken(tokenDescriptor);
    }

    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(secureRandomBytes);
        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
    {
        throw new NotImplementedException();
    }
}