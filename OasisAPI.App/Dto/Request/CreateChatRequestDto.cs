using Domain.Entities;

namespace OasisAPI.App.Dto.Request;

public class CreateChatRequestDto
{
    public string Message { get; set; } = string.Empty;
    public HashSet<ChatBotEnum> ChatBotEnums { get; set; } = [];
}