namespace OasisAPI.Models;

public class OasisMessage(string name, string message)
{
    public string? Name { get; set; } = name;
    public string Message { get; set; } = message;
    public string? ThreadId { get; set; }
    public string? MessageId { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
}
