using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OasisAPI.Interfaces.Services;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal ExtractClaimsFromAccessToken(string expiredAccessToken);
}