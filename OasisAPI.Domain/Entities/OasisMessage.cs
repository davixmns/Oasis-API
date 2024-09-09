using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class OasisMessage : BaseEntity
{
    public int? OasisChatId { get; set; }

    [Required] [StringLength(50)] 
    public string From { get; set; }

    [Required] 
    public string Message { get; set; }

    public string? FromThreadId { get; set; }

    public string? FromMessageId { get; set; }

    public bool? IsSaved { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public OasisChat? Chat { get; set; }

    public OasisMessage(string from, string message,
        string? fromMessageId = null,
        string? fromThreadId = null,
        int? oasisChatId = null,
        bool? isSaved = false
    )

    {
        From = from;
        Message = message;
        OasisChatId = oasisChatId;
        FromThreadId = fromThreadId;
        FromMessageId = fromMessageId;
        CreatedAt = DateTime.UtcNow;
        IsSaved = isSaved;
    }
    
    public OasisMessage()
    {
        CreatedAt = DateTime.UtcNow;
    }
}