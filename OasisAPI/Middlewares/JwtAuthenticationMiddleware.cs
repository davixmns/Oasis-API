using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
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
            return AuthenticateResult.Fail("No token provided.");

        var validatedToken = _tokenService.ValidateAccessToken(token);

        if (!validatedToken.Success)
            return AuthenticateResult.Fail("Invalid Token");

        var claimsPrincipal = new ClaimsPrincipal(validatedToken.Data!);
        
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return AuthenticateResult.Fail("Invalid token.");
        
        Context.Items["UserId"] = int.Parse(userId);
        
        var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        
        return AuthenticateResult.Success(ticket);
    }
}