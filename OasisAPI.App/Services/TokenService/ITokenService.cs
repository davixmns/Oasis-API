using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OasisAPI.App.Services.TokenService;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateAccessToken(string token);
    ClaimsPrincipal ExtractClaimsFromExpiredAccessToken(string expiredAccessToken);
    DateTime GetRefreshTokenExpiryDateTime();
}