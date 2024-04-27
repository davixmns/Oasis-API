using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OasisAPI.Config;
using OasisAPI.Dto;
using OasisAPI.Interfaces;

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
    public async Task<IActionResult> Login([FromBody] LoginModelDto loginData)
    {
        var userExists = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginData.Email!);

        if (userExists is null)
            return NotFound("User not found");

        var passwordIsCorrect = BCrypt.Net.BCrypt.Verify(loginData.Password!, userExists.Password);

        if (!passwordIsCorrect)
            return Unauthorized("Password is incorrect");

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

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            refreshToken,
            userExists.RefreshTokenExpiryDateTime
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> CreateNewAccessToken([FromBody] TokenModelDto tokenModelDto)
    {
        var principal = _tokenService.ExtractClaimsFromAccessToken(tokenModelDto.AccessToken!);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId is null) 
            return BadRequest("Invalid token");
        
        var userExists = await _unitOfWork.UserRepository.GetAsync(u => u.OasisUserId == int.Parse(userId));
        
        if (userExists is null || userExists.RefreshToken != tokenModelDto.RefreshToken ||
            DateTime.UtcNow < userExists.RefreshTokenExpiryDateTime || userExists.OasisUserId.ToString() != userId)
            return BadRequest("Invalid token");
        
        var accessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        userExists.RefreshToken = refreshToken;
        userExists.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddMinutes(_jwtConfig.Value.RefreshTokenExpiry!.Value);
        
        _unitOfWork.UserRepository.Update(userExists);
        await _unitOfWork.CommitAsync();
        
        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            refreshToken,
            userExists.RefreshTokenExpiryDateTime
        });
    }
}