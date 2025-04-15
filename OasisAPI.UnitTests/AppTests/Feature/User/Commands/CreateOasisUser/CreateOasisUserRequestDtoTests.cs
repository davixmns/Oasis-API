namespace OasisAPI.Tests.AppTests.Feature.User.Commands.CreateOasisUser;

using OasisAPI.App.Features.User.Commands.CreateOasisUser;
using Xunit;

public class CreateOasisUserRequestDtoTests
{

    [Fact]
    public void Should_Initialize_Correctly()
    {
        var name = "Test User";
        var email = "test@example.com";
        var password = "password123";

        var request = new CreateOasisUserRequestDto
        {
            Name = name,
            Email = email,
            Password = password
        };

        Assert.Equal(name, request.Name);
        Assert.Equal(email, request.Email);
        Assert.Equal(password, request.Password);
    }
}