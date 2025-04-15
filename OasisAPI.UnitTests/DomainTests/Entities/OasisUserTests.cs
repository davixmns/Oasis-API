namespace OasisAPI.Tests.DomainTests.Entities;

using Domain.Entities;

using Xunit;

public class OasisUserTests
{
    [Theory]
    [InlineData("Test User", "test@example.com", "password123")] // Dados válidos
    public void OasisUser_ShouldInitializeCorrectly_WithParameters(string name, string email, string password)
    {
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

    [Theory]
    [InlineData("", "test@example.com", "password123")]
    [InlineData("Test User", "", "password123")]
    [InlineData("Test User", "test@example.com", "")]
    public void OasisUser_ShouldThrowArgumentException_WhenInvalidData(string name, string email, string password)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new OasisUser(name, email, password));
    }

    [Theory]
    [InlineData("Test Chat")] // Título válido
    public void OasisUser_ShouldAddChat(string chatTitle)
    {
        // Arrange
        var user = new OasisUser("Test User", "test@example.com", "password123");

        // Act
        var chat = user.AddChat(chatTitle);

        // Assert
        Assert.NotNull(chat);
        Assert.Contains(chat, user.Chats);
        Assert.Equal(user.Id, chat.OasisUserId);
    }

    [Theory]
    [InlineData("")] // Título inválido
    public void AddChat_ShouldThrowArgumentException_WhenInvalidData(string chatTitle)
    {
        // Arrange
        var user = new OasisUser("Test User", "test@example.com", "password123");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => user.AddChat(chatTitle));
    }

    [Fact]
    public void OasisUser_ShouldGetAttributesCorrectly()
    {
        // Arrange
        var user = new OasisUser("Test User", "test user", "password123");
        
        // Act & Assert
        Assert.Equal("Test User", user.Name);
        Assert.Equal("test user", user.Email);
        Assert.Equal("password123", user.Password);
        Assert.Null(user.RefreshToken);
        Assert.True(user.RefreshTokenExpiryDateTime <= DateTime.UtcNow);
        Assert.NotNull(user.Chats);
        Assert.Empty(user.Chats);
    }
    
    [Fact]
    public void OasisUser_ShouldGetAttributesCorrectly_WhenEmptyConstructor()
    {
        // Arrange
        var user = new OasisUser();
        
        // Act & Assert
        Assert.Null(user.Name);
        Assert.Null(user.Email);
        Assert.Null(user.Password);
        Assert.Null(user.RefreshToken);
        Assert.True(user.RefreshTokenExpiryDateTime <= DateTime.UtcNow);
        Assert.NotNull(user.Chats);
        Assert.Empty(user.Chats);
    }

    [Fact]
    public void OasisUser_ShouldUpdateAttributesCorrectly()
    {
        // Arrange
        var user = new OasisUser("Test User", "test user", "password123");

        // Act
        user.Name = "New Name";
        user.Email = "new email";
        user.Password = "new password";

        // Assert
        Assert.Equal("New Name", user.Name);
        Assert.Equal("new email", user.Email);
        Assert.Equal("new password", user.Password);

    }
}
