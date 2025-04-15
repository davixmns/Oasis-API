using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Domain.Utils;

namespace Domain.Entities;

public class OasisChat : BaseEntity
{
    public string Title { get; set; }
    
    public int OasisUserId { get; set; }
    
    [JsonIgnore]
    public OasisUser? User { get; set; }

    public ICollection<OasisMessage> Messages { get; set; }
    
    public ICollection<OasisChatBotDetails> ChatBots { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public OasisChat(int oasisUserId, string title)
    {
        if(string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty", nameof(title));
        
        OasisUserId = oasisUserId;
        Title = title;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Messages = new Collection<OasisMessage>();
        ChatBots = new List<OasisChatBotDetails>();
    }

    public OasisMessage AddMessage(ChatBotEnum sender, string message)
    {
        if(string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty", nameof(message));
        
        var newMessage = new OasisMessage(sender, message);
        Messages!.Add(newMessage);
        UpdatedAt = DateTime.UtcNow;
        return newMessage;
    }
}