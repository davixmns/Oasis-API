using OasisAPI.Enums;

namespace OasisAPI.App.Dto.Request;

public class ChatBotAndThreadDto
{
    public ChatBotEnum ChatBotEnum { get; set; }
    public string threadId { get; set; }
}