using Domain.Utils;

namespace Domain.Entities;

public class OasisChatBotDetails : BaseEntity
{
    public int OasisChatId { get; set; }
    public ChatBotEnum ChatBotEnum { get; set; }
    public bool IsActive { get; set; }      
    public string? ThreadId { get; set; }     
    
    public OasisChatBotDetails(int oasisChatId, ChatBotEnum chatBotEnum, bool isActive, string? threadId)
    {
        OasisChatId = oasisChatId;
        ChatBotEnum = chatBotEnum;
        IsActive = isActive;
        ThreadId = threadId;
    }
}
