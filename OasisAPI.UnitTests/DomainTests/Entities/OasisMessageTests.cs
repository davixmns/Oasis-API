using Domain.Entities;
using Domain.Utils;

namespace OasisAPI.Tests.DomainTests.Entities;

using Xunit;

public class OasisMessageTests
{
    
    [Fact]
    public void OasisMessage_ShouldInitializeCorrectly_WithParameters()
    {
        //Act
        var message = new OasisMessage(ChatBotEnum.ChatGpt, "Test Message");
        
        //Assert
        Assert.NotNull(message);
        Assert.Equal(ChatBotEnum.ChatGpt, message.ChatBotEnum);
        Assert.Equal("Test Message", message.Message);
        Assert.True(message.CreatedAt <= DateTime.UtcNow);
        Assert.False(message.IsSaved);
    }
    
    [Fact]
    public void OasisMessage_ShouldThrowArgumentException_WhenInvalidData()
    {
        //Act & Assert
        Assert.Throws<ArgumentException>(() => new OasisMessage(ChatBotEnum.ChatGpt, ""));
    }
}