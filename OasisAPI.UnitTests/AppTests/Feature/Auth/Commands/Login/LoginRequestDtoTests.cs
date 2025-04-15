using OasisAPI.App.Features.Auth.Commands.Login;
using Xunit;
using Assert = Xunit.Assert;

namespace OasisAPI.Tests.AppTests.Feature.Auth.Commands.Login;

public class LoginRequestDtoTests
{
    [Fact]
    public void LoginRequestDto_ShouldInitializeCorrectly()
    {
        var email = "email@example.com";
        var password = "password";

        var loginRequestDto = new LoginRequestDto
        {
            Email = email,
            Password = password
        };

        Assert.Equal(email, loginRequestDto.Email);
        Assert.Equal(password, loginRequestDto.Password);
    }
    
    
    [Fact]
    public void LoginRequestDto_ShouldInitializeWithDefaultValues()
    {
        var loginRequestDto = new LoginRequestDto();

        Assert.Empty(loginRequestDto.Email);
        Assert.Empty(loginRequestDto.Password);
    }
}