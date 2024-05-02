using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOptions<JwtConfig> _jwtConfig;

    public AuthController(IUnitOfWork unitOfWork, ITokenService tokenService, IOptions<JwtConfig> jwtConfig)
    {
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _jwtConfig = jwtConfig;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginData)
    {
        var userExists = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginData.Email!);

        if (userExists is null)
            return NotFound(OasisApiResponse<string>.ErrorResponse("User not found"));

        var passwordIsCorrect = BCrypt.Net.BCrypt.Verify(loginData.Password!, userExists.Password);

        if (!passwordIsCorrect)
            return Unauthorized(OasisApiResponse<string>.ErrorResponse("Incorrect password"));

        List<Claim> userClaims =
        [
            new Claim(ClaimTypes.NameIdentifier, userExists.OasisUserId.ToString()),
            new Claim(ClaimTypes.Email, userExists.Email),
        ];

        var accessToken = _tokenService.GenerateAccessToken(userClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        userExists.RefreshToken = refreshToken;
        userExists.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddMinutes(_jwtConfig.Value.RefreshTokenExpiry!.Value);

        _unitOfWork.UserRepository.Update(userExists);
        await _unitOfWork.CommitAsync();
        
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
        var principal = _tokenService.ExtractClaimsFromExpiredAccessToken(tokenRequestDto.AccessToken!);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return BadRequest(OasisApiResponse<string>.ErrorResponse("Invalid token"));
        }
        
        var userExists = await _unitOfWork.UserRepository.GetAsync(u => u.OasisUserId == int.Parse(userId));

        if (userExists is null || userExists.RefreshToken != tokenRequestDto.RefreshToken ||
            DateTime.UtcNow < userExists.RefreshTokenExpiryDateTime || userExists.OasisUserId.ToString() != userId)
        {
            return BadRequest(OasisApiResponse<string>.ErrorResponse("Invalid token"));
        }
        
        var accessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        userExists.RefreshToken = refreshToken;
        userExists.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddMinutes(_jwtConfig.Value.RefreshTokenExpiry!.Value);
        
        _unitOfWork.UserRepository.Update(userExists);
        await _unitOfWork.CommitAsync();
        
        var tokenResponse = new TokenResponse()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken,
            RefreshTokenExpiryDateTime = userExists.RefreshTokenExpiryDateTime
        };
        
        return Ok(OasisApiResponse<TokenResponse>.SuccessResponse(tokenResponse));
    }
}