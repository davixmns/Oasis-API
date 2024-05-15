using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OasisAPI.Config;
using OasisAPI.Interfaces.Services;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;

namespace OasisAPI.Services;

public sealed class TokenService : ITokenService
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
        var credentials =
            new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);
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

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateIssuerSigningKey = true,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Reduce the default clock skew used for expiration
        };
        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException($"Token validation failed: {ex.Message}");
        }
    }


    public ClaimsPrincipal ExtractClaimsFromExpiredAccessToken(string expiredAccessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            ValidateLifetime = false
        };
        var principal = new JwtSecurityTokenHandler().ValidateToken(
            expiredAccessToken,
            tokenValidationParameters,
            out var validatedToken
        );
        if (validatedToken is not JwtSecurityToken securityToken ||
            !securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}