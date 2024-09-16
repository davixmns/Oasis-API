using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Domain.Entities;
using MediatR;
using OasisAPI.App.Dto.Response;
using OasisAPI.App.Interfaces.Services;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;
using OasisAPI.Infra.Utils;

namespace OasisAPI.App.Features.Auth.Login.Command;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AppResult<LoginResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    
    public LoginCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
    }
    
    public async Task<AppResult<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Email == request.Email);
        
        var passwordIsCorrect = PasswordHasher.Verify(request.Password, user!.Password);
        
        if (!passwordIsCorrect)
            return AppResult<LoginResponseDto>.Fail("Email or password is incorrect");

        List<Claim> userClaims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        ];
        
        var accessToken = _tokenService.GenerateAccessToken(userClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDateTime = _tokenService.GetRefreshTokenExpiryDateTime();
        
        _unitOfWork.GetRepository<OasisUser>().Update(user);
        
        await _unitOfWork.CommitAsync();
        
        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken,
            RefreshTokenExpiryDateTime = user.RefreshTokenExpiryDateTime,
            OasisUserResponse = _mapper.Map<OasisUserResponseDto>(user)
        };
        
        return AppResult<LoginResponseDto>.Success(loginResponseDto);
    }
}