using Domain.Entities;

namespace OasisAPI.App.Dto.Request;

public class MessageRequestDto
{
    public int OasisChatId { get; set; }
    public ChatBotEnum From { get; set; }
    public string Message { get; set; } = string.Empty;
    public HashSet<ChatBotEnum> ChatBotEnums { get; set; } = [];
}