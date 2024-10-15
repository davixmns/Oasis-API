namespace OasisAPI.Tests.DomainTests.Entities;

using Domain.Entities;

using Xunit;

public class OasisUserTests
{
    [Fact]
    public void OasisUser_ShouldInitializeCorrectly_WithParameters()
    {
        // Arrange
        const string name = "Test User";
        const string email = "test@example.com";
        const string password = "password123";

        // Act
        var user = new OasisUser(name, email, password);

        // Assert
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.Null(user.RefreshToken);
        Assert.True(user.RefreshTokenExpiryDateTime <= DateTime.UtcNow);
        Assert.NotNull(user.Chats);
        Assert.Empty(user.Chats);
    }
    
    [Fact]
    public void OasisUser_ShouldAddChat()
    {
        // Arrange 
        var user = new OasisUser("Test User", "test@example.com", "password123");
        var chat = user.AddChat("Test Chat");

        // Assert
        Assert.NotNull(user);
        Assert.NotNull(chat);
        Assert.Contains(chat, user.Chats);
        Assert.Equal(user.Id, chat.OasisUserId);
    }

    [Fact]
    public void OasisUser_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        const string name = "";
        const string email = "test@example.com";
        const string password = "password123";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new OasisUser(name, email, password));
    }

    [Fact]
    public void OasisUser_ShouldThrowArgumentException_WhenEmailIsEmpty()
    {
        // Arrange
        const string name = "Test User";
        const string email = "";
        const string password = "password123";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new OasisUser(name, email, password));
    }

    [Fact]
    public void OasisUser_ShouldThrowArgumentException_WhenPasswordIsEmpty()
    {
        // Arrange
        const string name = "Test User";
        const string email = "test@example.com";
        const string password = "";
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new OasisUser(name, email, password));
    }

    [Fact]
    public void AddChat_ShouldThrowArgumentException_WhenTitleIsEmpty()
    {
        // Arrange
        var user = new OasisUser("Test User", "test@example.com", "password123");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => user.AddChat(""));
    }
}
