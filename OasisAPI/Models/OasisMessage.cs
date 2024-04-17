using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OasisAPI.Models;

public class OasisMessage
{
    [Key]
    public int OasisMessageId { get; set; }
    
    public int OasisChatId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string From { get; set; }
    
    [Required]
    public string Message { get; set; }
    
    public string? FromThreadId { get; set; }
    
    public string? FromMessageId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [JsonIgnore]
    public OasisChat? Chat { get; set; }

    public OasisMessage(string from, string message, string? fromMessageId = null, string? fromThreadId = null)
    {
        From = from;
        Message = message;
        FromThreadId = fromThreadId;
        FromMessageId = fromMessageId;
        CreatedAt = DateTime.Now;
    }
}