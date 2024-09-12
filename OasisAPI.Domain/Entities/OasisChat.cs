using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.ValueObjects;
using OasisAPI.Enums;

namespace Domain.Entities;

public class OasisChat : BaseEntity
{
    [StringLength(50)]
    public string? Title { get; set; }
    
    public int OasisUserId { get; set; }
    
    [JsonIgnore]
    public OasisUser? User { get; set; }

    public ICollection<OasisMessage>? Messages { get; set; }

    public ICollection<OasisChatBotInfo> ChatBots { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public OasisChat(int oasisUserId, string? title = null, ICollection<OasisChatBotInfo>? chatBots = null)
    {
        OasisUserId = oasisUserId;
        Title = title;
        
        Messages = new Collection<OasisMessage>();
        ChatBots = chatBots ?? new Collection<OasisChatBotInfo>();
        CreatedAt = DateTime.UtcNow;
    }
}