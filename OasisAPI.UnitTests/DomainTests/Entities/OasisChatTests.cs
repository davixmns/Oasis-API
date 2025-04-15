using Domain.Utils;

namespace OasisAPI.Tests.DomainTests.Entities;

using Domain.Entities;
using Xunit;

public class OasisChatTests
{
    [Theory]
    [InlineData(1, "Test Chat")]
    public void OasisChat_ShouldInitializeCorrectly_WithParameters(int userId, string title)
    {
        //Act
        var chat = new OasisChat(userId, title);
        
        //Assert
        Assert.NotNull(chat);
        Assert.Equal(userId, chat.OasisUserId);
        Assert.Equal(title, chat.Title);
        Assert.True(chat.CreatedAt <= DateTime.UtcNow);
        Assert.True(chat.UpdatedAt <= DateTime.UtcNow);
        Assert.Empty(chat.Messages);
    }
    
    [Theory]
    [InlineData(0, "")]
    public void OasisChat_ShouldThrowArgumentException_WhenInvalidData(int userId, string title)
    {
        //Act & Assert
        Assert.Throws<ArgumentException>(() => new OasisChat(userId, title));
    }
    
    [Theory]
    [InlineData(ChatBotEnum.ChatGpt, "Test Message")]
    [InlineData(ChatBotEnum.Gemini, "Test Message")]
    public void OasisChat_ShouldAddMessage(ChatBotEnum sender, string message)
    {
        //Arrange
        var chat = new OasisChat(1, "Test Chat");
        
        //Act
        var newMessage = chat.AddMessage(sender, message);
        
        //Assert
        Assert.NotNull(newMessage);
        Assert.Contains(newMessage, chat.Messages);
        Assert.True(chat.UpdatedAt <= DateTime.UtcNow);
    }
    
    [Theory]
    [InlineData(ChatBotEnum.ChatGpt, "")]
    public void AddMessage_ShouldThrowArgumentException_WhenInvalidData(ChatBotEnum sender, string message)
    {
        //Arrange
        var chat = new OasisChat(1, "Test Chat");
        
        //Act & Assert
        Assert.Throws<ArgumentException>(() => chat.AddMessage(sender, message));
    }
    
}