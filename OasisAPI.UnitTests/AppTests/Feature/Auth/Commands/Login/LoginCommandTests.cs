using OasisAPI.App.Features.Auth.Commands.Login;

namespace OasisAPI.Tests.AppTests.Feature.Auth.Commands;

using Xunit;

public class LoginCommandTests
{

    [Fact]
    public void LoginCommand_ShouldInitializeCorrectly()
    {
        var email = "test@example.com";
        var password = "password";

        var command = new LoginCommand(email, password);

        Assert.Equal(email, command.Email);
        Assert.Equal(password, command.Password);
    }

    [Theory]
    [InlineData(null, "password")]
    [InlineData("", "password")]
    [InlineData("example@gmail.com", null)]
    [InlineData("example@gmail.com", "")]
    public void LoginCommand_ShouldThrowArgumentException_WhenInvalidData(string email, string password)
    {
        Assert.Throws<ArgumentException>(() => new LoginCommand(email, password));
    }
}