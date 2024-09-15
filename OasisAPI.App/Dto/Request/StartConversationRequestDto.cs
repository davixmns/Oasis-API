using Domain.Entities;

namespace OasisAPI.App.Dto.Request;

public class StartConversationRequestDto
{
    public string Message { get; init; } = string.Empty;
    public HashSet<ChatBotEnum> ChatBotEnums { get; init; } = [];
}