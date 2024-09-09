using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class OasisChat : BaseEntity
{
    [StringLength(50)]
    public string? Title { get; set; }
    
    public int OasisUserId { get; set; }
    
    [StringLength(100)]
    public string? ChatGptThreadId { get; set; }
    
    [StringLength(100)]
    public string? GeminiThreadId { get; set; }
    
    [JsonIgnore]
    public OasisUser? User { get; set; }
    
    public ICollection<OasisMessage>? Messages { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public OasisChat(
        int oasisUserId,
        string? chatGptThreadId = null,
        string? geminiThreadId = null, 
        string? title = null)
    {
        OasisUserId = oasisUserId;
        ChatGptThreadId = chatGptThreadId;
        GeminiThreadId = geminiThreadId;
        Messages = new Collection<OasisMessage>();
        CreatedAt = DateTime.UtcNow;
        Title = title;
    }
}