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
using OasisAPI.Utils;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOptions<JwtConfig> _jwtConfig;
    private readonly IMapper _mapper;

    public AuthController(IUnitOfWork unitOfWork, ITokenService tokenService,
        IOptions<JwtConfig> jwtConfig, IMapper mapper)
    {
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _jwtConfig = jwtConfig;
        _mapper = mapper;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var oasisUser = await _unitOfWork.UserRepository.GetAsync(u => u.Email == loginRequestDto.Email);

        if (oasisUser is null)
            return BadRequest(OasisApiResponse<string>.ErrorResponse("Email or password is incorrect"));

        var passwordIsCorrect = PasswordHasher.Verify(loginRequestDto.Password!, oasisUser.Password);

        if (!passwordIsCorrect)
            return BadRequest(OasisApiResponse<string>.ErrorResponse("Email or password is incorrect"));

        List<Claim> userClaims =
        [
            new Claim(ClaimTypes.NameIdentifier, oasisUser.OasisUserId.ToString()),
            new Claim(ClaimTypes.Email, oasisUser.Email),
        ];

        var accessToken = _tokenService.GenerateAccessToken(userClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        oasisUser.RefreshToken = refreshToken;
        oasisUser.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddMinutes(_jwtConfig.Value.RefreshTokenExpiry);

        _unitOfWork.UserRepository.Update(oasisUser);

        await _unitOfWork.CommitAsync();

        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken,
            RefreshTokenExpiryDateTime = oasisUser.RefreshTokenExpiryDateTime,
            OasisUserResponse = _mapper.Map<OasisUserResponseDto>(oasisUser)
        };

        return Ok(OasisApiResponse<LoginResponseDto>.SuccessResponse(loginResponseDto));
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> CreateNewAccessToken([FromBody] TokenRequestDto tokenRequestDto)
    {
        var principal = _tokenService.ExtractClaimsFromExpiredAccessToken(tokenRequestDto.AccessToken!);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null) 
            return BadRequest("Invalid token");

        var userExists = await _unitOfWork.UserRepository.GetAsync(u => u.OasisUserId == int.Parse(userId));

        if (userExists is null || userExists.RefreshToken != tokenRequestDto.RefreshToken ||
            DateTime.UtcNow < userExists.RefreshTokenExpiryDateTime || userExists.OasisUserId.ToString() != userId)
            return BadRequest("Invalid token");

        var accessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        userExists.RefreshToken = refreshToken;
        userExists.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddMinutes(_jwtConfig.Value.RefreshTokenExpiry);

        _unitOfWork.UserRepository.Update(userExists);

        await _unitOfWork.CommitAsync();

        var tokenResponse = new LoginResponseDto()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken,
            RefreshTokenExpiryDateTime = userExists.RefreshTokenExpiryDateTime
        };

        return Ok(tokenResponse);
    }

    [Authorize]
    [HttpGet("VerifyAccessToken")]
    public async Task<IActionResult> VerifyAccessToken()
    {
        var userId = int.Parse(HttpContext.Items["UserId"]!.ToString()!);
        
        var user = await _unitOfWork.UserRepository.GetAsync(u => u.OasisUserId == userId);
        
        var userDto = _mapper.Map<OasisUserResponseDto>(user);
        
        return Ok(OasisApiResponse<OasisUserResponseDto>.SuccessResponse(userDto));
    }
}