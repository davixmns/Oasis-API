using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Domain.ValueObjects;

namespace Domain.Entities;

public class OasisChat : BaseEntity
{
    public string Title { get; set; }
    
    public int OasisUserId { get; set; }
    
    [JsonIgnore]
    public OasisUser? User { get; set; }

    public ICollection<OasisMessage>? Messages { get; set; }
    
    public ICollection<OasisChatBotInfo> ChatBots { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public OasisChat(int oasisUserId, string title)
    {
        OasisUserId = oasisUserId;
        Title = title;
        CreatedAt = DateTime.UtcNow;
        Messages = new Collection<OasisMessage>();
        ChatBots = new Collection<OasisChatBotInfo>();
    }
}