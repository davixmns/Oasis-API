using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OasisAPI.Models;

public class OasisChat
{
    [Key]
    public int ChatId { get; set; }
    
    public int UserId { get; set; }
    
    [StringLength(100)]
    public string? ChatGptThreadId { get; set; }
    
    [StringLength(100)]
    public string? GeminiThreadId { get; set; }
    
    [JsonIgnore]
    public OasisUser? User { get; set; }
    
    public ICollection<OasisMessage>? Messages { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public OasisChat()
    {
        Messages = new Collection<OasisMessage>();
        CreatedAt = DateTime.UtcNow;
    }
}