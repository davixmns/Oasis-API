using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OasisAPI.Models;

[Table("oasis_chats")]
public class OasisChat
{
    [Key]
    public int OasisChatId { get; set; }
    
    [StringLength(50)]
    public string? Title { get; set; }
    
    public int UserId { get; set; }
    
    [StringLength(100)]
    public string? ChatGptThreadId { get; set; }
    
    [StringLength(100)]
    public string? GeminiThreadId { get; set; }
    
    [JsonIgnore]
    public OasisUser? User { get; set; }
    
    public ICollection<OasisMessage>? Messages { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public OasisChat(
        int userId,
        string? chatGptThreadId = null,
        string? geminiThreadId = null, 
        string? title = null)
    {
        UserId = userId;
        ChatGptThreadId = chatGptThreadId;
        GeminiThreadId = geminiThreadId;
        Messages = new Collection<OasisMessage>();
        CreatedAt = DateTime.UtcNow;
        Title = title;
    }
}