using Domain.Entities;

namespace OasisAPI.App.Dto.Request;

public class ChatBotAndThreadDto
{
    public ChatBotEnum ChatBotEnum { get; set; }
    public string? threadId { get; set; }
    
    public ChatBotAndThreadDto(ChatBotEnum chatBotEnum, string? threadId)
    {
        ChatBotEnum = chatBotEnum;
        this.threadId = threadId;
    }
}