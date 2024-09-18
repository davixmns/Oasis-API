using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using OasisAPI.App.Services.TokenService;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.Middlewares;

public class JwtAuthenticationMiddleware : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public JwtAuthenticationMiddleware(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUnitOfWork unitOfWork,
        ITokenService tokenService) : base(options, logger, encoder, clock)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var accessToken = GetAccessTokenFromHeader();
        var refreshToken = GetRefreshTokenFromHeader();

        if (string.IsNullOrWhiteSpace(accessToken))
            return AuthenticateResult.Fail("Access token not found.");

        var claimsPrincipal = _tokenService.ValidateAccessToken(accessToken);

        if (claimsPrincipal is null) // Token inválido
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return AuthenticateResult.Fail("Invalid token.");
            
            return await CreateNewTokensToUserAsync(accessToken, refreshToken!);
        }

        return ContinueWithRequest(claimsPrincipal);
    }

    private string? GetAccessTokenFromHeader()
    {
        return Request.Headers.Authorization.FirstOrDefault()?.Split(' ').Last();
    }
    
    private string? GetRefreshTokenFromHeader()
    {
        return Request.Headers["X-Refresh-Token"];
    }

    private async Task<AuthenticateResult> CreateNewTokensToUserAsync(string expiredAccessToken, string refreshToken)
    {
        var claimsPrincipal = _tokenService.ExtractClaimsFromExpiredAccessToken(expiredAccessToken);

        if (!TryGetUserId(claimsPrincipal, out var userId))
            return AuthenticateResult.Fail("Invalid token.");

        var user = await _unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Id == userId);

        if (user is null || !IsRefreshTokenValid(user, refreshToken))
            return AuthenticateResult.Fail("Invalid Refresh token.");

        // Gera novos tokens
        var newAccessToken = _tokenService.GenerateAccessToken(claimsPrincipal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // Atualiza tokens no banco
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryDateTime = _tokenService.GetRefreshTokenExpiryDateTime();
        
        _unitOfWork.GetRepository<OasisUser>().Update(user);
        await _unitOfWork.CommitAsync();

        // Retorna o novo AccessToken e RefreshToken
        Response.Headers.Append("X-New-Tokens", "true");
        Response.Headers.Append("X-New-Access-Token", new JwtSecurityTokenHandler().WriteToken(newAccessToken));
        Response.Headers.Append("X-New-Refresh-Token", newRefreshToken);

        return ContinueWithRequest(claimsPrincipal);
    }

    private static bool TryGetUserId(ClaimsPrincipal claimsPrincipal, out int userId)
    {
        var userIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdString, out userId);
    }

    private static bool IsRefreshTokenValid(OasisUser user, string refreshToken)
    {
        if (user.RefreshToken != refreshToken)
            return false;

        if (DateTime.UtcNow > user.RefreshTokenExpiryDateTime)
            return false;
        
        return true;
    }

    private AuthenticateResult ContinueWithRequest(ClaimsPrincipal claimsPrincipal)
    {
        if (!TryGetUserId(claimsPrincipal, out var userId))
            return AuthenticateResult.Fail("Invalid token.");

        Context.Items["UserId"] = userId;

        var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        
        return AuthenticateResult.Success(ticket);
    }
}
