using Domain.Entities;
using Domain.Utils;

namespace OasisAPI.App.Dto.Request;

public class StartConversationWithChatBotsDto
{
    public string Message { get; init; } = string.Empty;
    public HashSet<ChatBotEnum> ChatBotEnums { get; init; } = [];
}