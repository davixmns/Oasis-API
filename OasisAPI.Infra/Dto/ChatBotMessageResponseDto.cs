namespace OasisAPI.Infra.Dto;

public class ChatBotMessageResponseDto
{
    public string Message { get; set; }
    public string ChatBotName { get; set; }
    public string? ThreadId { get; set; }
    public string? MessageId { get; set; }
}