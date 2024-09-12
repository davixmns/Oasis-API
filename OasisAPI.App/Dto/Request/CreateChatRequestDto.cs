using OasisAPI.Enums;

namespace OasisAPI.Infra.Dto;

public class CreateChatRequestDto
{
    public string Message { get; set; } = string.Empty;
    public HashSet<ChatBotEnum> ChatBotEnums { get; set; } = [];
}