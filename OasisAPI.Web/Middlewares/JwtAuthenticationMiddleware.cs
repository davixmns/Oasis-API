using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.Interfaces.Services;

namespace OasisAPI.Middlewares;

public class JwtAuthenticationMiddleware : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ITokenService _tokenService;

    public JwtAuthenticationMiddleware(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ITokenService tokenService) : base(options, logger, encoder, clock)
    {
        _tokenService = tokenService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = Request.Headers.Authorization.FirstOrDefault()?.Split(' ').Last();
        
        if (token is null)
            return AuthenticateResult.Fail("Token not found.");

        var claimsPrincipal = _tokenService.ValidateAccessToken(token);

        if (claimsPrincipal is null)
            return AuthenticateResult.Fail("Invalid token.");
        
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return AuthenticateResult.Fail("Invalid token.");
        
        Context.Items["UserId"] = int.Parse(userId);
        
        var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        
        return AuthenticateResult.Success(ticket);
    }
}