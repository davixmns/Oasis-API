using OasisAPI.Models;

namespace OasisAPI.Interfaces.Services;

public interface IChatbotsService
{
    public Task<OasisMessage> StartGptChat(string userMessage);
    public Task<OasisMessage> StartGeminiChat(string userMessage);
    
    public Task<OasisMessage> SendMessageToGemini(string userMessage);
   
    public Task<OasisMessage> SendMessageToGpt(string userMessage, string threadId);
    
    
    public Task<OasisMessage> RetrieveGptMessage(string messageId, string threadId);

    public Task<OasisMessage> RetrieveChatTheme(string userMessage);
}