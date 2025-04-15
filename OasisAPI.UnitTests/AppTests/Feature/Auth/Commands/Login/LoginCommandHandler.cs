using OasisAPI.App.Features.Auth.Commands.Login;
using OasisAPI.App.Features.User.Queries.GetUserData;
using OasisAPI.App.Services.TokenService;
using OasisAPI.Infra.Repositories;
using OasisAPI.Infra.Utils;

namespace OasisAPI.Tests.AppTests.Feature.Auth.Commands.Login;

using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Domain.Entities;
using Moq;
using Xunit;


public class LoginCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly LoginCommandHandler _handler;
    private readonly Mock<IRepository<OasisUser>> _userRepositoryMock;
    private readonly OasisUser _userMock = new OasisUser("User name", "test@email.com", PasswordHasher.Hash("user_password"));

     public LoginCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tokenServiceMock = new Mock<ITokenService>();
        _mapperMock = new Mock<IMapper>();
        _userRepositoryMock = new Mock<IRepository<OasisUser>>();
        
        _unitOfWorkMock.Setup(u => u.GetRepository<OasisUser>())
            .Returns(_userRepositoryMock.Object);
        
        _userRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<OasisUser, bool>>>()))
            .ReturnsAsync(_userMock);

        _handler = new LoginCommandHandler(_unitOfWorkMock.Object, _tokenServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new LoginCommand("test@email.com", "user_password");
        
        _tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<List<Claim>>()))
            .Returns(new JwtSecurityToken());
        
        _tokenServiceMock.Setup(t => t.GenerateRefreshToken())
            .Returns("sample_refresh_token");
        
        _tokenServiceMock.Setup(t => t.GetRefreshTokenExpiryDateTime())
            .Returns(DateTime.UtcNow.AddDays(7));
        
        _mapperMock.Setup(m => m.Map<OasisUserResponseDto>(_userMock))
            .Returns(new OasisUserResponseDto {Email = _userMock.Email});

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(result.Data.OasisUserResponse.Email, _userMock.Email);
        Assert.Equal("sample_refresh_token", result.Data.RefreshToken);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = new LoginCommand("teste@email.com", "wrong_password");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Email or password is incorrect", result.Message);
    }
}