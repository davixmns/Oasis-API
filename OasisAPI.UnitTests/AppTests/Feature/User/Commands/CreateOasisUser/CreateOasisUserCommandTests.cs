using OasisAPI.App.Features.User.Commands.CreateOasisUser;

namespace OasisAPI.Tests.AppTests.Feature.User.Commands.CreateOasisUser;

using Xunit;

public class CreateOasisUserCommandTests
{
    
    [Fact]
    public void Should_Initialize_Correctly()
    {
        var name = "Test User";
        var email = "test@example.com";
        var password = "password123";
        
        var command = new CreateOasisUserCommand(name, email, password);
     
        Assert.Equal(name, command.Name);
        Assert.Equal(email, command.Email);
        Assert.Equal(password, command.Password);
    }
}