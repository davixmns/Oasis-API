using Domain.Entities;

namespace OasisAPI.Infra.Dto;

public class ChatBotMessageResponseDto
{
    public string Message { get; set; }
    public ChatBotEnum ChatBotEnum { get; set; }
    public string? ThreadId { get; set; }
    public string? MessageId { get; set; }
}