using Microsoft.Extensions.Options;
using OasisAPI.Config;
using OasisAPI.Interfaces.Services;

namespace OasisAPI.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtConfig _jwtConfig;

    public JwtMiddleware(RequestDelegate next, IOptions<JwtConfig> jwtConfig)
    {
        _next = next;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task Invoke(HttpContext context, ITokenService tokenService)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, tokenService, token);

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, ITokenService tokenService, string token)
    {
        try
        {
            var claims = tokenService.ExtractClaimsFromAccessToken(token);
            context.Items["User"] = claims;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
        }
    }
}