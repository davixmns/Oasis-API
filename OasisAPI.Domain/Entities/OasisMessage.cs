using Domain.Utils;

namespace Domain.Entities;

public class OasisMessage : BaseEntity
{
    public int? OasisChatId { get; set; }
    public ChatBotEnum ChatBotEnum { get; set; }
    public string Message { get; set; }
    public bool? IsSaved { get; set; }
    public DateTime CreatedAt { get; set; }

    public OasisMessage(ChatBotEnum chatBotEnum, string message, int? oasisChatId = null, bool? isSaved = false)
    {
        ChatBotEnum = chatBotEnum;
        Message = message;
        OasisChatId = oasisChatId;
        CreatedAt = DateTime.UtcNow;
        IsSaved = isSaved;
    }
    
    public OasisMessage()
    {
        CreatedAt = DateTime.UtcNow;
    }
}