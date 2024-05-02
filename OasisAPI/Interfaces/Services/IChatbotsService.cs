using OasisAPI.Models;

namespace OasisAPI.Interfaces.Services;

public interface IChatbotsService
{
    public Task<OasisMessage> StartGptChat(string userMessage);
    public Task<OasisMessage> StartGeminiChat(string userMessage);
    
    public Task<OasisMessage> SendMessageGpt(string userMessage, string threadId);
    public Task<OasisMessage> SendMessageGemini(string userMessage, string threadId);
    
    public Task<IQueryable<OasisMessage>> RetrieveGeminiMessageList(string threadId);
    public Task<OasisMessage> RetrieveGptMessage(string messageId, string threadId);
}