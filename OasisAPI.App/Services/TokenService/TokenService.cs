using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OasisAPI.App.Config;
using OasisAPI.App.Interfaces.Services;
using SigningCredentials = Microsoft.IdentityModel.Tokens.SigningCredentials;

namespace OasisAPI.App.Services;

public sealed class TokenService : ITokenService
{
    private readonly JwtConfig _jwtConfig;

    public TokenService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var privateKey = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256Signature
        );
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiry),
            Audience = _jwtConfig.Audience,
            Issuer = _jwtConfig.Issuer,
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

    public ClaimsPrincipal? ValidateAccessToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = _jwtConfig.Issuer,
            ValidateIssuerSigningKey = true,
            ValidAudience = _jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };
        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
        catch (Exception)
        {
            return null;
        }
    }


    public ClaimsPrincipal ExtractClaimsFromExpiredAccessToken(string expiredAccessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
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
    
    public DateTime GetRefreshTokenExpiryDateTime()
    {
        return DateTime.UtcNow.AddMinutes(_jwtConfig.RefreshTokenExpiry);
    }
}