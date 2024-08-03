using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OasisAPI.Models;

namespace OasisAPI.Interfaces.Services;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    OasisApiResponse<ClaimsPrincipal> ValidateAccessToken(string token);
    ClaimsPrincipal ExtractClaimsFromExpiredAccessToken(string expiredAccessToken);
}