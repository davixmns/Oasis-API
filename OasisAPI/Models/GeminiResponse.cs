using GenerativeAI.Methods;

namespace OasisAPI.Models;

public class GeminiResponse
{
    public ChatSession chatSession { get; set; }
    public string message { get; set; }
    
    public GeminiResponse(ChatSession chatSession, string message)
    {
        this.chatSession = chatSession;
        this.message = message;
    }
}