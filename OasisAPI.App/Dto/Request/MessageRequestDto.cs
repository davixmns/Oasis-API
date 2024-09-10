using OasisAPI.Enums;

namespace OasisAPI.App.Dto.Request;

public class MessageRequestDto
{
    public string Message { get; set; } = string.Empty;
    public HashSet<ChatBotsEnum> ChatBotEnums { get; set; } = [];
}