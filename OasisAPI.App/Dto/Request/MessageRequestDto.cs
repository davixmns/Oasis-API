using OasisAPI.Enums;

namespace OasisAPI.App.Dto.Request;

public class MessageRequestDto
{
    public int ChatId { get; set; }
    public string Message { get; set; } = string.Empty;
    public HashSet<ChatBotEnum> ChatBotEnums { get; set; } = [];
}