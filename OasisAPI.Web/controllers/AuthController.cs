using MediatR;
using Microsoft.AspNetCore.Mvc;
using OasisAPI.App.Commands;
using OasisAPI.App.Dto.Request;

namespace OasisAPI.controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var command = new LoginCommand(dto.Email, dto.Password);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }

    // [HttpPost("Login")]
    // public async Task<IActionResult> Login(LoginRequestDto dto)
    // {
    //     var oasisUser = await _unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Email == dto.Email);
    //
    //     if (oasisUser is null)
    //         return BadRequest(AppResult<string>.ErrorResponse("Email or password is incorrect"));
    //
    //     var passwordIsCorrect = PasswordHasher.Verify(dto.Password!, oasisUser.Password);
    //
    //     if (!passwordIsCorrect)
    //         return BadRequest(AppResult<string>.ErrorResponse("Email or password is incorrect"));
    //
    //     List<Claim> userClaims =
    //     [
    //         new Claim(ClaimTypes.NameIdentifier, oasisUser.Id.ToString()),
    //         new Claim(ClaimTypes.Email, oasisUser.Email),
    //     ];
    //
    //     var accessToken = _tokenService.GenerateAccessToken(userClaims);
    //     var refreshToken = _tokenService.GenerateRefreshToken();
    //
    //     oasisUser.RefreshToken = refreshToken;
    //     oasisUser.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddMinutes(_jwtConfig.Value.RefreshTokenExpiry);
    //
    //     _unitOfWork.GetRepository<OasisUser>().Update(oasisUser);
    //
    //     await _unitOfWork.CommitAsync();
    //
    //     var loginResponseDto = new LoginResponseDto()
    //     {
    //         AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
    //         RefreshToken = refreshToken,
    //         RefreshTokenExpiryDateTime = oasisUser.RefreshTokenExpiryDateTime,
    //         OasisUserResponse = _mapper.Map<OasisUserResponseDto>(oasisUser)
    //     };
    //
    //     return Ok(AppResult<LoginResponseDto>.SuccessResponse(loginResponseDto));
    // }

    // [HttpPost("RefreshToken")]
    // public async Task<IActionResult> CreateNewAccessToken([FromBody] TokenRequestDto tokenRequestDto)
    // {
    //     var principal = _tokenService.ExtractClaimsFromExpiredAccessToken(tokenRequestDto.AccessToken!);
    //     var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //
    //     if (userId is null) 
    //         return BadRequest("Invalid token");
    //
    //     var userExists = await _unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Id == int.Parse(userId));
    //
    //     if (userExists is null || userExists.RefreshToken != tokenRequestDto.RefreshToken ||
    //         DateTime.UtcNow < userExists.RefreshTokenExpiryDateTime || userExists.Id.ToString() != userId)
    //         return BadRequest("Invalid token");
    //
    //     var accessToken = _tokenService.GenerateAccessToken(principal.Claims);
    //     var refreshToken = _tokenService.GenerateRefreshToken();
    //
    //     userExists.RefreshToken = refreshToken;
    //     userExists.RefreshTokenExpiryDateTime = DateTime.UtcNow.AddMinutes(_jwtConfig.Value.RefreshTokenExpiry);
    //
    //     _unitOfWork.GetRepository<OasisUser>().Update(userExists);
    //
    //     await _unitOfWork.CommitAsync();
    //
    //     var tokenResponse = new LoginResponseDto()
    //     {
    //         AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
    //         RefreshToken = refreshToken,
    //         RefreshTokenExpiryDateTime = userExists.RefreshTokenExpiryDateTime
    //     };
    //
    //     return Ok(tokenResponse);
    // }
    //
    // [Authorize]
    // [HttpGet("VerifyAccessToken")]
    // public async Task<IActionResult> VerifyAccessToken()
    // {
    //     var userId = int.Parse(HttpContext.Items["UserId"]!.ToString()!);
    //     
    //     var user = await _unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Id == userId);
    //     
    //     var userDto = _mapper.Map<OasisUserResponseDto>(user);
    //     
    //     return Ok(AppResult<OasisUserResponseDto>.SuccessResponse(userDto));
    // }
}