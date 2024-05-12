using Microsoft.AspNetCore.Mvc;
using OasisAPI.Models;
using OpenAI;
using OpenAI.Threads;

namespace OasisAPI.Interfaces.Services;

public interface IChatbotsService
{
    public Task<OasisMessage> CreateGptChat(string userMessage);
    public Task<OasisMessage> CreateGeminiChat(string userMessage);
    
    public Task<OasisMessage> SendMessageToGeminiChat(string userMessage);
   
    public Task<OasisMessage> SendMessageToGptChat(string threadId, string userMessage);
    
    
    public Task<OasisMessage> RetrieveGptMessage(string messageId, string threadId);

    public Task<OasisMessage> RetrieveChatTheme(string userMessage);
}