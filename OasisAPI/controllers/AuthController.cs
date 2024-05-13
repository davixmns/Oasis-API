using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OasisAPI.Config;
using OasisAPI.Dto;
using OasisAPI.Interfaces;
using OasisAPI.Interfaces.Services;
using OasisAPI.Models;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService tokenService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IOptions<JwtConfig> jwtConfig;
    private readonly IMapper mapper;

    public AuthController(IUnitOfWork unitOfWork, ITokenService tokenService,
        IOptions<JwtConfig> jwtConfig, IMapper mapper)
    {
        this.tokenService = tokenService;
        this.unitOfWork = unitOfWork;
        this.jwtConfig = jwtConfig;
        this.mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginData)
    {
        var userExists = await this.unitOfWork
            .UserRepository
            .GetAsync(u => u.Email == loginData.Email);
        
        if (userExists is null)
        {
            return NotFound(OasisApiResponse<string>.ErrorResponse("User not found"));
        }

        var passwordIsCorrect = BCrypt.Net.BCrypt.Verify(loginData.Password!, userExists.Password);

        if (!passwordIsCorrect)
        {
            return Unauthorized(OasisApiResponse<string>.ErrorResponse("Incorrect password"));
        }

        List<Claim> userClaims =
        [
            new Claim(ClaimTypes.NameIdentifier, userExists.OasisUserId.ToString()),
            new Claim(ClaimTypes.Email, userExists.Email),
        ];

        var accessToken = tokenService.GenerateAccessToken(userClaims);
        var refreshToken = tokenService.GenerateRefreshToken();

        userExists.RefreshToken = refreshToken;
        userExists.RefreshTokenExpiryDateTime = DateTime
            .UtcNow
            .AddMinutes(jwtConfig.Value.RefreshTokenExpiry!.Value);

        this.unitOfWork
            .UserRepository
            .Update(userExists);

        await this.unitOfWork
            .CommitAsync();
        
        var tokenResponse = new TokenResponse()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken,
            RefreshTokenExpiryDateTime = userExists.RefreshTokenExpiryDateTime,
            OasisUser = new OasisUserDto()
            {
                OasisUserId = userExists.OasisUserId,
                Name = userExists.Name!,
                Email = userExists.Email
            }
        };

        return Ok(OasisApiResponse<TokenResponse>.SuccessResponse(tokenResponse));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> CreateNewAccessToken([FromBody] TokenRequestDto tokenRequestDto)
    {
        var principal = tokenService.ExtractClaimsFromExpiredAccessToken(tokenRequestDto.AccessToken!);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return BadRequest(OasisApiResponse<string>.ErrorResponse("Invalid token"));
        }

        var userExists = await this.unitOfWork
            .UserRepository
            .GetAsync(u => u.OasisUserId == int.Parse(userId));

        if (userExists is null || userExists.RefreshToken != tokenRequestDto.RefreshToken ||
            DateTime.UtcNow < userExists.RefreshTokenExpiryDateTime || userExists.OasisUserId.ToString() != userId)
        {
            return BadRequest(OasisApiResponse<string>.ErrorResponse("Invalid token"));
        }

        var accessToken = tokenService.GenerateAccessToken(principal.Claims);
        var refreshToken = tokenService.GenerateRefreshToken();

        userExists.RefreshToken = refreshToken;
        userExists.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddMinutes(jwtConfig.Value.RefreshTokenExpiry!.Value);

        this.unitOfWork
            .UserRepository
            .Update(userExists);

        await this.unitOfWork.CommitAsync();

        var tokenResponse = new TokenResponse()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken,
            RefreshTokenExpiryDateTime = userExists.RefreshTokenExpiryDateTime
        };

        return Ok(OasisApiResponse<TokenResponse>.SuccessResponse(tokenResponse));
    }

    [Authorize]
    [HttpGet("VerifyAccessToken")]
    public async Task<IActionResult> VerifyAccessToken()
    {
        var userId = int.Parse(HttpContext.Items["UserId"]!.ToString()!);
        var user = await this.unitOfWork
            .UserRepository
            .GetAsync(u => u.OasisUserId == userId);
        var userDto = mapper.Map<OasisUserDto>(user);
        return Ok(OasisApiResponse<OasisUserDto>.SuccessResponse(userDto));
    }
}