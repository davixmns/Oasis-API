namespace OasisAPI.Models;

public class ConversationRequest
{
    public string? Model { get; set; }
    public string? Message { get; set; }
    public string? AssistantId { get; set; }
    public string? ThreadId { get; set; }
    public float? Temperature { get; set; }
    public int? MaxTokens { get; set; }
}