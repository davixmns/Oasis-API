namespace OasisAPI.Tests.DomainTests.Entities;

using Domain.Entities;
using Xunit;

public class OasisChatTests
{
    [Fact]
    public void OasisChat_ShouldInitializeCorrectly_WithParameters()
    {
        //Arrange
        const int userId = 10;
        const string title = "Title";
        
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
}