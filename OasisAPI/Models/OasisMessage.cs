namespace OasisAPI.Models;

public class OasisMessage
{
    public string Name { get; set; }
    public string Message { get; set; }
    public string? ThreadId { get; set; }
    public string? MessageId { get; set; }
    public DateTime DateTime { get; set; }

    public OasisMessage(string name, string message, string? threadId = null, string? messageId = null)
    {
        Name = name;
        Message = message;
        ThreadId = threadId;
        MessageId = messageId;
        DateTime = DateTime.UtcNow;
    }
}