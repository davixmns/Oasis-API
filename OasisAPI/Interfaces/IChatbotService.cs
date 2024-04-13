using OasisAPI.Models;

namespace OasisAPI.Interfaces;

public interface IChatbotService
{
    public Task<OasisMessage> StartChat(string userMessage);
    public Task<OasisMessage> SendMessage(string userMessage, string threadId);
    public Task<IQueryable<OasisMessage>> RetrieveMessageList(string threadId);
    public Task<OasisMessage> RetrieveMessage(string messageId, string threadId);
}