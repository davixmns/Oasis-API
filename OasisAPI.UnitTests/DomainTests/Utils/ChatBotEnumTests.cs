using Domain.Utils;

namespace OasisAPI.Tests.DomainTests.Utils;
using Xunit;

public class ChatBotEnumTests
{
    [Theory]
    [InlineData(ChatBotEnum.User, 0)]
    [InlineData(ChatBotEnum.ChatGpt, 1)]
    [InlineData(ChatBotEnum.Gemini, 2)]
    public void ChatBotEnum_ShouldInitializeCorrectly(ChatBotEnum chatBotEnum, int expected)
    {
        Assert.Equal(expected, (int) chatBotEnum);
    }
}