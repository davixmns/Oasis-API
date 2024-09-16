using Domain.Utils;

namespace Domain.Entities;

public class OasisMessage : BaseEntity
{
    public int? OasisChatId { get; set; }
    public ChatBotEnum From { get; set; }
    public string Message { get; set; }
    public bool? IsSaved { get; set; }
    public DateTime CreatedAt { get; set; }

    public OasisMessage(ChatBotEnum from, string message, int? oasisChatId = null, bool? isSaved = false)
    {
        From = from;
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