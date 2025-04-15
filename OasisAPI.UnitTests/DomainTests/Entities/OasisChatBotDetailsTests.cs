using Domain.Entities;
using Domain.Utils;

namespace OasisAPI.Tests.DomainTests.Entities;

using Xunit;

public class OasisChatBotDetailsTests
{
    [Theory]
    [InlineData(1, ChatBotEnum.ChatGpt, true, "TestThread")]
    public void ShouldInitializeCorrectly_WithParameters(int oasisChatId, ChatBotEnum chatBotEnum, bool isActive, string? threadId)
    {
        //Act
        var chatBotDetails = new OasisChatBotDetails(oasisChatId, chatBotEnum, isActive, threadId);
        
        //Assert
        Assert.NotNull(chatBotDetails);
        Assert.Equal(oasisChatId, chatBotDetails.OasisChatId);
        Assert.Equal(chatBotEnum, chatBotDetails.ChatBotEnum);
        Assert.Equal(isActive, chatBotDetails.IsActive);
        Assert.Equal(threadId, chatBotDetails.ThreadId);
    }
}