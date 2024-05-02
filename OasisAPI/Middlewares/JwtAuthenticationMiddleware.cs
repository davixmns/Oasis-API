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

        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("No token provided.");
        }
        
        try
        {
            var claims = _tokenService.ValidateToken(token);
            var principal = new ClaimsPrincipal(claims);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
        }
    }
    
    
}